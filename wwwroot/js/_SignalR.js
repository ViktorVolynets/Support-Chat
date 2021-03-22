


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .withAutomaticReconnect()
    .build();

hubConnection.serverTimeoutInMilliseconds = 1000 * 60 * 10; // 1 second * 60 * 10 = 10 minutes.
let userName = "UserDefault";
let message = { name: userName };

// получение сообщения от сервера
hubConnection.on("Receive", function (mes) {

    message.dialogid = mes.dialogid;

    let servmessage = document.createElement("div");
    //console.dir(mes)
    if (mes.senderType == "in") {

        servmessage.className = "chat first";
        //addMessage(mes, 'first', mes.name, elem);
    }
    else {

        servmessage.className = "chat second";
        //addMessage(mes, 'second', mes.name, elem.outerHTML);
    }
    servmessage.appendChild(document.createTextNode('Name:' + mes.name));
    // создает элемент <p> для сообщения пользователя
    let elem = document.createElement("p");
    var t = "";
        if (mes.textTupe == "json") {

        jsontext = JSON.parse(mes.text);
        elem.appendChild(document.createTextNode(jsontext.text));
        servmessage.appendChild(elem);
        if (jsontext.buttoncount > 0) {
            var index;
            for (index = 0; index < jsontext.textbutton.length; ++index) {
                var button = document.createElement("button");
                button.innerHTML = jsontext.textbutton[index];
                button.classList.add("btn-interract");
                button.classList.add("btn");
                button.classList.add("btn-primary");

                servmessage.appendChild(button);
                elem.appendChild(button);
                button.onclick = UserButtonInterracts;

                var br = document.createElement("br");
                servmessage.appendChild(br);
            } 

        }
        var str = "              <span class=\"time\">" + new Date().toLocaleString() + "<\/span>";
        elem.insertAdjacentHTML('beforeend', str);
    }
    else {
        elem.appendChild(document.createTextNode(mes.text));
        var str = "              <span class=\"time\">" + new Date().toLocaleString() + "<\/span>";
        elem.insertAdjacentHTML('beforeend', str);
        servmessage.appendChild(elem);
    }

 

    var firstElem = document.getElementById("chat-messages").firstChild;
    document.getElementById("chat-messages").insertBefore(servmessage, firstElem);
});
hubConnection.start();
