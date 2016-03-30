using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using ADServerDAL.Abstract;
using ADServerManagementWebApplication.Controllers;

namespace ADServerManagementWebApplication.Extensions
{
	/// <summary>
	/// Rozszerzenia
	/// </summary>
	public static class AccountExtesions
	{
		#region -IPrincipal-

		/// <summary>
		/// Pobranie roli Usera
		/// </summary>
		/// <param name="item">User</param>
		/// <returns>Rola użytkownika w postaci string</returns>
		public static string GetRole(this IPrincipal item)
		{
			try
			{
				return ((ClaimsIdentity)item.Identity).FindFirst(ClaimTypes.Role).Value;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		/// <summary>
		/// Pobranie Id Usera
		/// </summary>
		/// <param name="item">User</param>
		/// <returns>Id użytkownika w postaci string</returns>
		public static string GetUserID(this IPrincipal item)
		{
			try
			{
				return ((ClaimsIdentity)item.Identity).FindFirst(ClaimTypes.NameIdentifier).Value;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		/// <summary>
		/// Pobranie Id Usera
		/// </summary>
		/// <param name="item">User</param>
		/// <returns>Id użytkownika w postaci int</returns>
		public static int GetUserIDInt(this IPrincipal item)
		{
			try
			{
				return int.Parse(((ClaimsIdentity)item.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
			}
			catch
			{
				return 0;
			}
		}

		/// <summary>
		/// Pobranie ilości AdPoints użytkownika
		/// </summary>
		/// <param name="item"></param>
		/// <returns>AdPoints</returns>
        public static string GetAdPoints(this IPrincipal item)
        {
            try
            {
                return ((ClaimsIdentity)item.Identity).FindFirst("AdPoints").Value;
            }
            catch (Exception)
            {
                return "0.0";
            }
        }

        public static string GetLongName(this IPrincipal item)
        {
            try
            {
                return ((ClaimsIdentity)item.Identity).FindFirst("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider").Value;
            }
            catch (Exception)
            {
                return "0.0";
            }
        }


        public static void UpdateAdPoints(this IPrincipal item, IUsersRepository usersRepository)
		{
			try
			{
				if (item == null) return;
				var id = item.GetUserIDInt();
				var user = usersRepository.Users.First(it => it.Id == id);
				var claim = ((ClaimsIdentity) item.Identity).FindFirst("AdPoints");
				((ClaimsIdentity) item.Identity).RemoveClaim(claim);

				var newer = new Claim("AdPoints", user.AdPoints.ToString(CultureInfo.GetCultureInfo("en-US")));
				((ClaimsIdentity) item.Identity).AddClaim(newer);
			}
			catch (Exception e)
			{
				
			}
		}
		public static void UpdateAdPoints(this IPrincipal item, decimal value)
		{
			try
			{
				if (item == null) return;
				var claim = ((ClaimsIdentity)item.Identity).FindFirst("AdPoints");
				((ClaimsIdentity)item.Identity).RemoveClaim(claim);

				var newer = new Claim("AdPoints", value.ToString(CultureInfo.GetCultureInfo("en-US")));
				((ClaimsIdentity)item.Identity).AddClaim(newer);
			}
			catch (Exception e)
			{

			}
		}

		#endregion -IPrincipal-
	}
}