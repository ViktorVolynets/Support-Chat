"use strict";
window.onload = Init();

function Init() {
    $("#WorkTime").change(function () {
        //console.dir($("#WorkTime").val());
        $("#WorkTimeTo").val($("#WorkTime").val());
        //console.dir($("#WorkTimeTo").val());
    });
}