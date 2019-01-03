using System.Security.Claims;
using GetShredded.Common;
using GetShredded.Services.Contracts;
using GetShredded.ViewModel.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetShredded.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        public MessagesController(IMessageService messageService)
        {
            this.MessageService = messageService;
        }

        protected IMessageService MessageService { get; }

        [HttpGet]
        public IActionResult Information(string username)
        {
            var model = this.MessageService.Information(username);

            return this.View(model);
        }

        [HttpGet]
        public IActionResult DeleteMessage(int id)
        {
            this.MessageService.DeleteMessage(id);

            return RedirectToAction("Information", "Messages", new { username = this.User.Identity.Name });
        }

        [HttpGet]
        public IActionResult DeleteAllMessages(string username)
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.MessageService.DeleteAllMessages(userId);

            return RedirectToAction("Information", "Messages", new { username = this.User.Identity.Name });
        }

        public IActionResult MarkAllMessagesAsSeen(string username)
        {
            this.MessageService.AllMessagesSeen(username);

            return RedirectToAction("Information", "Messages", new { username });
        }

        [HttpPost]
        public IActionResult SendMessage(MessageInputModel inputModel)
        {
            if (string.IsNullOrWhiteSpace(inputModel.Message))
            {
                this.TempData[GlobalConstants.Error] = GlobalConstants.EmptyMessage;
                return RedirectToAction("Profile", "Users", new { username = inputModel.ReceiverName });
            }

            this.MessageService.SendMessage(inputModel);

            return RedirectToAction("Profile", "Users", new { username = inputModel.ReceiverName });
        }

        public IActionResult MessageSeen(int id)
        {
            this.MessageService.MessageSeen(id);

            return RedirectToAction("Information", "Messages", new { username = this.User.Identity.Name });
        }
    }
}
