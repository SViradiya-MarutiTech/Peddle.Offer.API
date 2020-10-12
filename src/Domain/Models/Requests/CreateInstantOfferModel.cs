﻿namespace Api.Models
{
    public class CreateInstantOfferModel
    {
        public int Id { get; set; }
        public double OfferAmount { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
    }
}
