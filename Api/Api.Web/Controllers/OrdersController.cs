namespace Api.Web.Controllers
{
    using Api.Models.DeliveryData;
    using Api.Models.Order;
    using Api.Models.OrderLog;
    using Api.Models.Shared;
    using Api.Services.Interfaces;
    using Api.Web.Infrastructure.Constants;
    using Api.Web.Models.Config;
    using Api.Web.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [Produces("application/json")]
    [Route("api/[controller]")]
    public class OrdersController : BaseController
    {
        private readonly IOrderService orders;

        private readonly IDeliveryDataService deliveryData;

        private readonly IOrderLogService logs;

        private readonly IMailService mails;

        private readonly SmtpConfiguration smtpConfiguration;

        public OrdersController(IOrderService orders, IDeliveryDataService deliveryData, IOrderLogService logs, IMailService mails, IOptions<SmtpConfiguration> smtpConfiguration, IUserService users) : base(users)
        {
            this.orders = orders;
            this.deliveryData = deliveryData;
            this.logs = logs;
            this.mails = mails;
            this.smtpConfiguration = smtpConfiguration.Value;
        }

        //post api/orders
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody]OrderWithoutUserCreateModel order)
        {
            return await this.Execute(false, true, async () =>
            {
                string orderId = await this.orders.Create(order, this.UserId);

                OrderDetailsModel orderModel = await this.orders.Get(orderId);

                string subject = string.Format(MailConstants.SubjectCreate, orderModel.Number);

                string receiverMail = MailConstants.OfficeMail;

                #if DEBUG
                    receiverMail= MailConstants.DebugMail;
                #endif

                this.mails.Send(receiverMail, subject, "", smtpConfiguration);

                return this.Ok(new { orderId = orderId });
            });
        }

        //put api/orders/id
        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> EditOrder(string id, [FromBody]OrderWithoutUserEditModel order)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.orders.Edit(id, this.UserId, order);

                return this.Ok(new { orderId = id });
            });
        }

        //get api/orders/id
        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                OrderDetailsModel order = await this.orders.Get(id);

                return this.Ok(new { order = order });
            });
        }

        //get api/orders/logs/id
        [HttpGet]
        [Route("logs/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLog(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                ICollection<OrderLogDetailsModel> list = await logs.GetLog(id);

                return this.Ok(new { logs = list });
            });
        }

        //get api/orders
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get([FromQuery]PaginationModel pagination)
        {
            if (pagination.FilterElement == null) pagination.FilterElement = "";

            if (pagination.FilterValue == null) pagination.FilterValue = "";

            if (pagination.SortElement == null) pagination.SortElement = "";

            return await this.Execute(true, true, async () =>
            {
                OrderDetailsListPaginatedModel result = await orders.GetAll(pagination);

                return this.Ok(result);
            });     
        }

        //post api/orders/confirm/id
        [HttpPost]
        [Route("confirm/{id}")]
        [Authorize]
        public async Task<IActionResult> Confirm(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                OrderDetailsModel order = await this.orders.ChangeStatus(id, this.UserId, Domain.Enums.OrderStatus.Confirmed);

                if (!await this.orders.IsConfirmationMailSent(id))
                {
                    DeliveryDataDetailsModel deliveryData = await this.deliveryData.Get(order.DeliveryDataId);

                    string userEmail = deliveryData.Email;
                    string subject = string.Format(MailConstants.SubjectConfirm, order.Number);

                    this.mails.Send(userEmail, subject, MailConstants.ContentConfirm, this.smtpConfiguration);

                    await this.orders.SetConfirmationMailSent(id);
                }

                return this.Ok();
            });
        }

        //post api/orders/dispatch/id
        [HttpPost]
        [Route("dispatch/{id}")]
        [Authorize]
        public async Task<IActionResult> Dispatch(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.orders.ChangeStatus(id, this.UserId, Domain.Enums.OrderStatus.Dispatched);

                return this.Ok();
            });
        }

        //post api/orders/cancel/id
        [HttpPost]
        [Route("cancel/{id}")]
        [Authorize]
        public async Task<IActionResult> Cancel(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                OrderDetailsModel order = await this.orders.ChangeStatus(id, this.UserId, Domain.Enums.OrderStatus.Cancelled);
                DeliveryDataDetailsModel deliveryData = await this.deliveryData.Get(order.DeliveryDataId);

                string userEmail = deliveryData.Email;
                string subject = string.Format(MailConstants.SubjectCancel, order.Number);

                this.mails.Send(userEmail, subject, MailConstants.ContentCancel, this.smtpConfiguration);

                return this.Ok();
            });
        }

        //post api/orders/reset/id
        [HttpPost]
        [Route("reset/{id}")]
        [Authorize]
        public async Task<IActionResult> ResetStatus(string id)
        {
            return await this.Execute(true, true, async () =>
            {
                await this.orders.ChangeStatus(id, this.UserId, Domain.Enums.OrderStatus.Ordered);

                return this.Ok();
            });
        }
    }
}

