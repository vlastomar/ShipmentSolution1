namespace ShipmentSolution.GCommon
{
    public static class ValidationConstants
    {
        public static class Customer
        {
            public const int NameMaxLength = 255;
            public const int EmailMaxLength = 255;
            public const int PhoneMaxLength = 255;
            public const int LocationMaxLength = 255;
        }

        public static class Shipment
        {
            public const int DimensionsMaxLength = 255;
            public const int ShippingMethodMaxLength = 255;
        }

        public static class MailCarrier
        {
            public const int NameMaxLength = 255;
            public const int EmailMaxLength = 255;
            public const int PhoneMaxLength = 255;
            public const int LocationMaxLength = 255;
            public const int StatusMaxLength = 255;
        }

        public static class Route
        {
            public const int LocationMaxLength = 255;
        }
    }
}