using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, IMessageRepository messageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessageDTO(CreateMessageDTO createMessageDTO)
        {
            var username = User.GetUsername();

            if (username == createMessageDTO.RecipientUsername.ToLower())
                return BadRequest("You can not send messages to youself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);

            if (recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.Username,
                RecipientUsername = recipient.Username,
                Content = createMessageDTO.Content
            };

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok(_mapper.Map<MessageDTO>(message));

            return BadRequest("Faild to send message");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageForUser([FromQuery] MessageParams messageParams)
        {
            messageParams.Username = User.GetUsername();

            var messages = await _messageRepository.GetMessageForUser(messageParams);

            Response.AddPaginationHeader(messages.CurrnetPage, messages.PageSize, messages.TotalCount, messages.TotalPages);

            return messages;
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
        {
            var currentUsername = User.GetUsername();
            return Ok(await _messageRepository.GetMessageThread(currentUsername, username));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            var username = User.GetUsername();
            var message = await _messageRepository.GetMessage(id);

            if (message.Sender.Username != username && message.Recipient.Username != username)
                return Unauthorized();

            // message display control for client
            if (message.Sender.Username == username) message.SenderDeleted = true;
            if (message.Recipient.Username == username) message.RecipientDeleted = true;

            // both delete the message, then delete from the server.
            if (message.SenderDeleted && message.RecipientDeleted)
                _messageRepository.DeleteMessage(message);

            if (await _messageRepository.SaveAllAsync()) return Ok();

            return BadRequest("Problem delete the message");
        }
    }
}