"use strict";

var http = new XMLHttpRequest();
http.open("GET", "/tweethuburl");
http.send();

var tweethuburl = null;

http.onreadystatechange = function () {
    if (this.readyState == 4 && this.status == 200) {
        tweethuburl = this.responseText;
        var mylabel = document.getElementById("hola")
        mylabel.textContent = tweethuburl
        connectToHub(tweethuburl);
    }
}

function connectToHub(hubUrl) {
    var connection = new signalR.HubConnectionBuilder().withUrl(hubUrl).build();

    connection.on("ReceiveTweet", function (message) {
        var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
        var encodedMsg = msg;
        var li = document.createElement("li");
        li.textContent = encodedMsg;
        document.getElementById("messagesList").appendChild(li);
    });

    connection.start()
        .catch(function (err) {
            return console.error(err.toString());
        });
}


