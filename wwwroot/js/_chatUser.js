"use strict";
window.onload = Init();
var user = new User(2);

function UserButtonInterracts(elem) {
    //console.dir(elem);
    //либо тут  добавлять
    hubConnection.invoke("Send", { "Text": this.innerHTML, "Name": userName, "SenderType": "in" });

      //SwapChatPanel();
      //$(".chat-button-row").empty();
}

// отправка сообщения от простого пользователя
document.getElementById("chat-submit").addEventListener("click", function (e) {
    let text = document.getElementById("chat-input").value;
    let mes = {};
    mes.name = message.name;
    mes.text = text;
    hubConnection.invoke("Send", mes);
});


function Init() {
    $(".minimizeChat").click(ToggleChat);
    var span = document.getElementsByClassName("closeChat")[0];
    span.onclick = function () {
        CloseChat();
    };

    RegisterModal();
    RegisterInput();

    // установка имени пользователя
    document.getElementById("loginBtn").addEventListener("click", function (e) {
        var width = $("#user-chat-container").width();
        userName = document.getElementById("userName").value;
        document.getElementById("header").innerHTML = "<h3>Welcome " + userName + "</h3>";
        message.name = userName;
        $("#userNameBlock").hide();
        $("#user-chat-container").width = width;
    });
    $(".enterName").click({ msg: "Profile" },UserProfileData);
    $(".enterEmail").click({ msg: "Profile" },UserProfileData);

}

function UserProfileData(e) {
    if (e.data.msg ==="Profile") {
        $("#modal-name").val(userName);
        $(".modal-body").children()[0].style.display = "none";
        $(".modal-header").children()[0].innerHTML = e.data.msg;
        $("#modalNo").hide();
    }
    $("#Modal").show();
}

function ModalSetDefault(){
    $(".modal-body").children()[0].style.display = "block";
    $(".modal-header").children()[0].innerHTML = "Do you want to complete this dialog ?";
    $("#modalNo").show();
    userName = $("#modal-name").val();
    document.getElementById("header").innerHTML = "<h3>Welcome " + userName + "</h3>";
    message.name = userName;
    //console.dir($("#modal-name"));
    $("#modal-name").val(userName);
        ToggleChat();
}


function ToggleChat(){
    $("#user-chat-container").toggle();
    $("#ChatOpenButton").toggle();
}

function CloseChat() {
    $("#Modal").toggle();
    $('.reason-buttons').find('[data-reason="' + user.Reasone + '"]')[0].focus(); //.addClass('active')
    //console.dir(sf);
}

function RegisterModal() {
    var modal = document.getElementById("Modal");
    var btn = document.getElementById("modalYes");
   

    btn.addEventListener("click", function () {
        $("#Modal").toggle();
        ToggleChat();
        ModalSetDefault();
         //OnDisconnect();
    }, false);

    btn = document.getElementById("modalNo");
    btn.addEventListener("click", function () { modal.style.display = "none"; }, false);
    var span = document.getElementsByClassName("closeModal")[0];
    span.onclick = function () {
        modal.style.display = "none";
    };
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    };
}

function RegisterInput() {
    $("#chat-submit").click(function (e) {
        e.preventDefault();
        var msg = $("#chat-input").val();
        if (msg.trim() == '') {
            return false;
        }
        //addMessage(msg, 'first','username','sometimes');

       // SwapChatPanel();
      //  AddBtn("asdasd");
      //  AddBtn("back to chat", "btn-success");

        //setTimeout(function () {
        //    addMessage(msg, 'second', 'connectedSpecName','tooLate');
        //}, 1000);

    });
    $("#chat-input").keyup("keyup", function (event) {
        // Number 13 is the "Enter" key on the keyboard
        if (event.keyCode === 13) {
            // Cancel the default action, if needed
            event.preventDefault();
            // Trigger the button element with a click
            document.getElementById("chat-submit").click();
        }
    });

}

function addMessage(msg, type, name,buttons) {
    var str = "";
    str += "<div  class=\"chat " + type + "\">";
    if (type === "first") {
        str += "              <i class=\"fas fa-users\"><\/i>";
    }
    else {
        str += "            <img src=\"https://www.w3schools.com/howto/img_avatar.png\" alt=\"Avatar\" class=\"right\">";
    }
    str += "              <span class=\"name\">" + name + "<\/span>";
    //str += "              <p class=\"chat\">" + msg.text + "<\/p>";
    console.dir("xxxxxxxxxx")
    console.dir(msg)
    str += "           " + buttons.OuterHTML + "";

    str += "              <span class=\"time time-right\">" + new Date().toLocaleString() + "<\/span>";
    str += "          <\/div>";

    document.getElementById("chat-messages").insertAdjacentHTML('afterbegin', str);
    //$(".chat-messages").append(str);
    if (type == 'first') {
        $("#chat-input").val('');
    }
    $(".chat-messages").stop().animate({ scrollTop: $(".chat-messages")[0].scrollHeight }, 1000);
}

function SwapChatPanel() {
    $(".chat-input").toggle();
    $(".chat-button-container").toggle();
}


function SetReasone(elem) {
    //console.dir(elem.dataset.reason);
    user.Reasone = elem.dataset.reason; 
    //console.dir(user.Reasone );

}

function AddBtn(btnText, btnClass = "btn-primary",btnRow =0) {
var button = document.createElement("button");
    button.innerHTML = btnText;
    button.classList.add("btn-interract");
    button.classList.add("btn");
    button.classList.add(btnClass);
    // Добавлять  другую  стату через параметры или дата аттр или как либо иначе
    var body = document.getElementsByClassName("chat-button-row")[btnRow];
    //console.dir(body);
    body.appendChild(button);
    button.onclick =UserButtonInterract;
}

function UserButtonInterract(elem) {
    //console.dir(elem);
    //либо тут  добавлять
    SwapChatPanel();
    $(".chat-button-row").empty();
}
