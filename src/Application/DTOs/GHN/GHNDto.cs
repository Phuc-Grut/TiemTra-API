using System.Text.Json.Serialization;

namespace Application.DTOs.GHN
{
    public sealed class GhnEnvelope<T>
    {
        public int Code { get; set; }               // 200 nếu OK
        public string? Message { get; set; }        // thông báo lỗi nếu có
        public T? Data { get; set; }                // dữ liệu thực tế
    }

    public sealed class GhnCalculateFeeRequest
    {
        [JsonPropertyName("from_district_id")]
        public int FromDistrictId { get; set; }

        [JsonPropertyName("from_ward_code")]
        public string FromWardCode { get; set; } = "";

        [JsonPropertyName("to_district_id")]
        public int ToDistrictId { get; set; }

        [JsonPropertyName("to_ward_code")]
        public string ToWardCode { get; set; } = "";

        [JsonPropertyName("service_id")]
        public int? ServiceId { get; set; }

        [JsonPropertyName("service_type_id")]
        public int? ServiceTypeId { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("insurance_value")]
        public int? InsuranceValue { get; set; }

        [JsonPropertyName("coupon")]
        public string? Coupon { get; set; }

        [JsonPropertyName("items")]
        public List<GhnItem>? Items { get; set; }
    }


    public sealed class GhnCalculateFeeResponse
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("service_fee")]
        public int ServiceFee { get; set; }

        [JsonPropertyName("insurance_fee")]
        public int InsuranceFee { get; set; }

        [JsonPropertyName("r2s_fee")]
        public int? R2SFee { get; set; }

        [JsonPropertyName("coupon_value")]
        public int? CouponValue { get; set; }

        [JsonPropertyName("pick_station_fee")]
        public int? PickStationFee { get; set; }
    }


    public class GhnItem
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }
    }

}
