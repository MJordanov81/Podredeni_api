namespace Api.Services.Implementations
{
    using Api.Data;
    using Api.Domain.Entities;
    using Api.Models.DeliveryData;
    using Api.Services.Infrastructure.Constants;
    using Api.Services.Interfaces;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;

    public class DeliveryDataService : IDeliveryDataService
    {
        private readonly ApiDbContext db;

        public DeliveryDataService(ApiDbContext db)
        {
            this.db = db;
        }

        public async Task<string> Create(DeliveryDataCreateModel data)
        {
            string officeDeliveryDataId = null;
            string homeDeliveryDataId = null;

            if (data.DeliveredToAnOffice)
            {
                officeDeliveryDataId = await this.CreateOfficeDeliveryData(data.OfficeCode, data.OfficeName, data.OfficeAddress, data.OfficeCity, data.OfficeCountry);
            }
            else
            {
                homeDeliveryDataId = await this.CreateHomeDeliveryData(data.Country, data.City, data.PostCode, data.Street, data.StreetNumber, data.District, data.Block, data.Entrance, data.Floor, data.Apartment);
            }

            string customerDataId = await this.CreateCustomerData(data.CustomerName, data.CustomerLastName, data.PhoneNumber, data.Email);

            DeliveryData thisData = new DeliveryData
            {
                Comments = data.Comments,
                DeliveredToAnOffice = data.DeliveredToAnOffice,
                OfficeDeliveryDataId = officeDeliveryDataId,
                HomeDeliveryDataId = homeDeliveryDataId,
                CustomerDataId = customerDataId
            };

            await this.db.DeliveryData.AddAsync(thisData);

            await this.db.SaveChangesAsync();

            return thisData.Id;
        }

        public async Task<string> Edit(string deliveryDataId, DeliveryDataCreateModel data)
        {
            DeliveryData deliveryData = await this.db.DeliveryData.FindAsync(deliveryDataId);

            if (deliveryData == null) throw new ArgumentException(ErrorMessages.InvalidDeliveryDataId);

            deliveryData.Comments = data.Comments;

            CustomerData customerData = await this.db.CustomerData.FindAsync(deliveryData.CustomerDataId);

            customerData.CustomerName = data.CustomerName;
            customerData.CustomerLastName = data.CustomerLastName;
            customerData.PhoneNumber = data.PhoneNumber;
            customerData.Email = data.Email;

            await this.db.SaveChangesAsync();

            if (deliveryData.DeliveredToAnOffice && data.DeliveredToAnOffice)
            {
                OfficeDeliveryData officeData = await this.db.OfficeDeliveryData.FindAsync(deliveryData.OfficeDeliveryDataId);

                officeData.Address = data.OfficeAddress;
                officeData.Name = data.OfficeName;
                officeData.City = data.OfficeCity;
                officeData.Country = data.OfficeCountry;
                officeData.Code = data.OfficeCode;

                await this.db.SaveChangesAsync();
            }
            else if (!deliveryData.DeliveredToAnOffice && !data.DeliveredToAnOffice)
            {
                HomeDeliveryData homeData = await this.db.HomeDeliveryData.FindAsync(deliveryData.HomeDeliveryDataId);

                homeData.PostCode = data.PostCode;
                homeData.City = data.City;
                homeData.Country = data.Country;
                homeData.District = data.District;
                homeData.Street = data.Street;
                homeData.StreetNumber = data.StreetNumber;
                homeData.Block = data.Block;
                homeData.Entrance = data.Entrance;
                homeData.Floor = data.Floor;
                homeData.Apartment = data.Apartment;

                await this.db.SaveChangesAsync();
            }
            else if (!deliveryData.DeliveredToAnOffice && data.DeliveredToAnOffice)
            {
                deliveryData.DeliveredToAnOffice = data.DeliveredToAnOffice;

                HomeDeliveryData homeData = await this.db.HomeDeliveryData.FindAsync(deliveryData.HomeDeliveryDataId);

                this.db.HomeDeliveryData.Remove(homeData);

                OfficeDeliveryData newData = new OfficeDeliveryData
                {
                    Address = data.OfficeAddress,
                    Name = data.OfficeName,
                    City = data.OfficeCity,
                    Country = data.OfficeCountry,
                    Code = data.OfficeCode
                };

                await this.db.OfficeDeliveryData.AddAsync(newData);            

                await this.db.SaveChangesAsync();

                deliveryData.OfficeDeliveryDataId = newData.Id;
            }
            else
            {
                deliveryData.DeliveredToAnOffice = data.DeliveredToAnOffice;

                OfficeDeliveryData officeData = await this.db.OfficeDeliveryData.FindAsync(deliveryData.OfficeDeliveryDataId);

                this.db.OfficeDeliveryData.Remove(officeData);

                HomeDeliveryData newData = new HomeDeliveryData
                {
                    PostCode = data.PostCode,
                    City = data.City,
                    District = data.District,
                    Street = data.Street,
                    StreetNumber = data.StreetNumber,
                    Block = data.Block,
                    Floor = data.Floor,
                    Apartment = data.Apartment
                };

                await this.db.HomeDeliveryData.AddAsync(newData);

                await this.db.SaveChangesAsync();

                deliveryData.HomeDeliveryDataId = newData.Id;
            }

            await this.db.SaveChangesAsync();

            return deliveryDataId;
        }

        public async Task<DeliveryDataDetailsModel> Get(string id)
        {
            if (!this.db.DeliveryData.Any(dd => dd.Id == id)) throw new ArgumentException(ErrorMessages.InvalidDeliveryDataId);

            DeliveryData data = await this.db.DeliveryData.FindAsync(id);

            CustomerData customer = await this.db.CustomerData.FindAsync(data.CustomerDataId);

            if (data.DeliveredToAnOffice)
            {
                OfficeDeliveryData office = await this.db.OfficeDeliveryData.FindAsync(data.OfficeDeliveryDataId);

                return this.db.DeliveryData
                    .Where(dd => dd.Id == id)
                    .Select(dd => new DeliveryDataDetailsModel
                    {
                        CustomerName = customer.CustomerName,
                        CustomerLastName = customer.CustomerLastName,
                        PhoneNumber = customer.PhoneNumber,
                        Email = customer.Email,
                        Comments = data.Comments,
                        DeliveredToAnOffice = data.DeliveredToAnOffice,
                        OfficeCode = office.Code,
                        OfficeName = office.Name,
                        OfficeAddress = office.Address,
                        OfficeCity = office.City,
                        OfficeCountry = office.Country
                    })
                .FirstOrDefault();
            }
            else
            {
                HomeDeliveryData home = await this.db.HomeDeliveryData.FindAsync(data.HomeDeliveryDataId);

                return this.db.DeliveryData
                    .Where(dd => dd.Id == id)
                    .Select(dd => new DeliveryDataDetailsModel
                    {
                        CustomerName = customer.CustomerName,
                        CustomerLastName = customer.CustomerLastName,
                        PhoneNumber = customer.PhoneNumber,
                        Email = customer.Email,
                        Country = home.Country,
                        City = home.City,
                        PostCode = home.PostCode,
                        Street = home.Street,
                        StreetNumber = home.StreetNumber,
                        District = home.District,
                        Block = home.Block,
                        Entrance = home.Entrance,
                        Floor = home.Floor,
                        Apartment = home.Apartment,
                        Comments = data.Comments,
                        DeliveredToAnOffice = data.DeliveredToAnOffice,

                    })
                .FirstOrDefault();
            }
        }

        private async Task<string> CreateCustomerData(string customerName, string customerLastName, string phoneNumber, string email)
        {
            if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(email)) throw new ArgumentException(ErrorMessages.InvalidCustomerData);

            CustomerData data = new CustomerData
            {
                CustomerName = customerName,
                CustomerLastName = customerLastName,
                PhoneNumber = phoneNumber,
                Email = email
            };

            await this.db.CustomerData.AddAsync(data);

            return data.Id;
        }

        private async Task<string> CreateHomeDeliveryData(string country, string city, string postCode, string street, string streetNumber, string district, string block, string entrance, string floor, string apartment)
        {
            HomeDeliveryData data = new HomeDeliveryData
            {
                Country = country,
                City = city,
                PostCode = postCode,
                Street = street,
                StreetNumber = streetNumber,
                District = district,
                Block = block,
                Entrance = entrance,
                Floor = floor,
                Apartment = apartment
            };

            await this.db.HomeDeliveryData.AddAsync(data);

            return data.Id;
        }

        private async Task<string> CreateOfficeDeliveryData(string officeCode, string officeName, string officeAddress, string officeCity, string officeCountry)
        {
            OfficeDeliveryData data = new OfficeDeliveryData
            {
                Code = officeCode,
                Name = officeName,
                Address = officeAddress,
                City = officeCity,
                Country = officeCountry
            };

            await this.db.OfficeDeliveryData.AddAsync(data);

            return data.Id;
        }
    }
}
