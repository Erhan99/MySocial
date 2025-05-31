"use strict";

const loggedUserId = localStorage.getItem("Logged");
const baseUrl = "https://localhost:7164/api/";
var messages = [];

const card = (message) => {
    const isMe = message.sender.id === loggedUserId;
    const bgClass = isMe ? 'bg-primary text-white' : 'bg-light text-dark';
    const marginClass = isMe ? 'ms-auto' : 'me-auto';
    const dateTextColor = isMe ? 'text-white ' : 'text-muted';

    return `
    <div class="d-flex my-3 px-3">
      <div class="p-3 rounded-pill shadow-sm ${bgClass} w-auto ${marginClass}">
        <div id="post-content-${message.id}" class="d-flex gap-1">
          <p class="mb-0">${message.content}</p>
          <small class="timestamp ${dateTextColor} mt-2 ms-2">
          ${new Date(message.createdAt).toLocaleString(undefined, {
              hour: 'numeric',
              minute: '2-digit'
          })}
        </small>
        </div>
      </div>
    </div>
  `;
};

const formatDay = (isoString) =>
    new Date(isoString).toLocaleDateString(undefined, {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric',
    });

const renderDateSeparator = (dayString) => `
  <div class="text-center my-3">
    <small class="bg-secondary text-white rounded-pill px-3 py-1">
      ${dayString}
    </small>
  </div>
`;

const FetchMessages = async () => {
    const user = document.querySelector("#UserId").value;
    const receiver = document.querySelector("#ReceiverId").value;
    const messageList = document.getElementById("messagesList");
    let previousDay = null;
    try {
        const response = await axios.get(baseUrl + "message", {
            params: {
                userId1: user,
                userId2: receiver
            },
            withCredentials: true
        });
        messages = response.data
        messages.forEach(message => {
            const currentDay = formatDay(message.createdAt);
            if (currentDay != previousDay) {
                messageList.innerHTML += renderDateSeparator(currentDay);
                previousDay = currentDay;
            }
            messageList.innerHTML += card(message)
        });
    } catch (error) {
        console.log("error fetching messages " + error);
    }
};

const CreateMessage = async (content) => {
    const user = document.querySelector("#UserId").value;
    const receiver = document.querySelector("#ReceiverId").value;
    const messageList = document.getElementById("messagesList")
    try {
        const response = await axios.post(baseUrl + "message", {
            Content: content,
            ReceiverId: receiver,
            SenderId: user
        },
        {withCredentials: true});
        const message = response.data
        messages.push(message)
        messageList.innerHTML += card(message)
    } catch (error) {
        console.log("error posting message " + error);
    }
};


var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    CreateMessage(message);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you
    // should be aware of possible script injection concerns.
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.addEventListener("DOMContentLoaded",FetchMessages)