#region

using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Hsr.Core;
using Hsr.Core.Cache;
using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Models;
using ServiceStack.Text;

#endregion

namespace Hsr.Controllers
{
    public class AccountController : HsrBaseController
    {
        private readonly IRepository<AreaInfo> _areaInfoRepository;
        private readonly IRepository<Authority> _authority;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Menu> _menuRepository;
        private readonly IRepository<UserSummaryInfo> _userRepository;

        public AccountController(IRepository<UserSummaryInfo> userRepository, IRepository<AreaInfo> areaInfoRepository
            , IRepository<Authority> authority, IRepository<Menu> menuRepository, ICacheManager cacheManager)
        {
            _areaInfoRepository = areaInfoRepository;
            _menuRepository = menuRepository;
            _cacheManager = cacheManager;
            _authority = authority;
            _userRepository = userRepository;
            PermissionCheck = false;
        }

        // GET: /Account/Login

        [System.Web.Mvc.AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
                
            return View("LoginNew");
        }

        //
        // POST: /Account/Login

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
#if DEBUG
            
            var currentUser =
                _userRepository.Table.SingleOrDefault(d => d.UserName == model.UserName && d.UserPw == model.Password);
            if (currentUser != null)
            {
                CurrentUser = currentUser;
                var roleIds = currentUser.Role.Select(r => r.Role.RoleId);
                var menuIds =
                    _authority.TableNoTracking.Where(d => roleIds.Contains(d.RoleId))
                        .Select(d => d.Menuid)
                        .Distinct()
                        .ToList();
                if (menuIds.Any())
                {
                    Menus = _menuRepository.TableNoTracking.Where(d =>d.Isenabled==1&& menuIds.Contains(d.Id)||!d.Layer.HasValue).ToList();
                 
                }
                else
                {
                    if (Menus != null)
                    {
                        Menus.Clear();
                    }
                }
                if (currentUser.AreaId.HasValue)
                {
                    UserBelong = _areaInfoRepository.TableNoTracking.SingleOrDefault(d => d.AreaId == currentUser.AreaId);
                }
                else
                {
                    if (currentUser.CityId.HasValue)
                    {
                        UserBelong =
                            _areaInfoRepository.TableNoTracking.SingleOrDefault(d => d.CityId == currentUser.CityId);
                    }
                    else
                    {
                        if (currentUser.ProvinceId.HasValue)
                        {
                            UserBelong =
                                _areaInfoRepository.TableNoTracking.SingleOrDefault(
                                    d => d.ProvinceId == currentUser.ProvinceId);
                        }
                    }
                }
                Session.Timeout = 60;
                Session[CommonHelper.UserName] = currentUser.UserName;
                CurrentUser = currentUser;
                //var cookie = new HttpCookie(CommonHelper.UserName, model.UserName);
                //cookie.Expires = DateTime.Now.AddMinutes(2);
                //Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Authority");
            }

            ModelState.AddModelError("", "提供的用户名或密码不正确");
            return View("LoginNew", model);
#endif
        }

        //
        // POST: /Account/LogOff

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.RemoveAll();
           
            var httpCookie = Request.Cookies[CommonHelper.UserName];
            if (httpCookie != null) httpCookie.Value = null;
            return RedirectToAction("Login");
        }

        #region 帮助程序

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // 请参见 http://go.microsoft.com/fwlink/?LinkID=177550 以查看
            // 状态代码的完整列表。
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "用户名已存在。请输入其他用户名。";

                case MembershipCreateStatus.DuplicateEmail:
                    return "该电子邮件地址的用户名已存在。请输入其他电子邮件地址。";

                case MembershipCreateStatus.InvalidPassword:
                    return "提供的密码无效。请输入有效的密码值。";

                case MembershipCreateStatus.InvalidEmail:
                    return "提供的电子邮件地址无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidAnswer:
                    return "提供的密码取回答案无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidQuestion:
                    return "提供的密码取回问题无效。请检查该值并重试。";

                case MembershipCreateStatus.InvalidUserName:
                    return "提供的用户名无效。请检查该值并重试。";

                case MembershipCreateStatus.ProviderError:
                    return "身份验证提供程序返回了错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                case MembershipCreateStatus.UserRejected:
                    return "已取消用户创建请求。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";

                default:
                    return "发生未知错误。请验证您的输入并重试。如果问题仍然存在，请与系统管理员联系。";
            }
        }

     

        #endregion
    }
}