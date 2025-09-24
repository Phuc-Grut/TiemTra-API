using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace Shared.Utilities.VNPAY
{
    public class NetworkHelper
    {
        /// <summary>
        /// Lấy địa chỉ IP từ HttpContext của API Controller.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetIpAddress(HttpContext context)
        {
            //var remoteIpAddress = context.Connection.RemoteIpAddress;

            //if (remoteIpAddress != null)
            //{
            //    var ipv4Address = Dns.GetHostEntry(remoteIpAddress).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);

            //    return remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6 && ipv4Address != null
            //        ? ipv4Address.ToString()
            //        : remoteIpAddress.ToString();
            //}

            //throw new InvalidOperationException("Không tìm thấy địa chỉ IP");
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(ip))
            {
                if (ip.Contains(","))
                {
                    ip = ip.Split(',')[0];
                }

                return ip;
            }

            var remoteIpAddress = context.Connection.RemoteIpAddress;

            if (remoteIpAddress != null)
            {
                return remoteIpAddress.IsIPv4MappedToIPv6
                    ? remoteIpAddress.MapToIPv4().ToString()
                    : remoteIpAddress.ToString();
            }

            throw new InvalidOperationException("Không tìm thấy địa chỉ IP");
        }
    }
}
