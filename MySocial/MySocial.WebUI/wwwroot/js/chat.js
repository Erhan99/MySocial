"use strict";

const baseUrl = "https://localhost:7164/api/";
var messages = [];

const card = (message) => {
    return (
        `<div class="container bg-white my-3 rounded-1 shadow-sm post" style="max-width:600px;">
      <div class="d-flex align-items-start p-3">
        <img src="/${message.sender.profilePictureUrl}" alt="Profile Picture" class="rounded-circle me-3" width="50" height="50" />

        <div class="flex-grow-1">
          <div class="d-flex justify-content-between align-items-center">
            <div>
              <strong class="me-2">${message.sender.userName}</strong>
              <span class="text-muted me-2">·</span>
              <small class="text-muted timestamp">
                ${new Date(message.createdAt).toLocaleString(undefined, {
            day: 'numeric',
            month: 'long',
            year: 'numeric',
            hour: 'numeric',
            minute: '2-digit'
        })}
              </small>
            </div>
        </div>
                  <div id="post-content-${message.id}" class="text-xl-start mb-2">
            <p class="mb-2">${message.content}</p>
          </div>
      </div>
    </div>
    `
    )
}

const FetchMessages = async () => {
    const user = document.querySelector("#UserId").value;
    const receiver = document.querySelector("#ReceiverId").value;
    const messageList = document.getElementById("messagesList")
    try {
        const response = await axios.get(baseUrl + "message", {
            params: {
                userId1: user,
                userId2: receiver
            }
        });
        console.log(response.data)
        messages = response.data
        messages.forEach(message => {
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
        });
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