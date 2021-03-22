function openDialog(elem) {
    // Объявить все переменные
    var i, tabcontent, tablinks;

    // Получить все элементы с помощью class="tabcontent" and hide them
    tabcontent = document.getElementsByClassName("tabcontent");
    for (i = 0; i < tabcontent.length; i++) {
        tabcontent[i].style.display = "none";
    }

    // Получить все элементы с помощью class="tablinks" и удалить класс "active"
    tablinks = document.getElementsByClassName("tablinks");
    for (i = 0; i < tablinks.length; i++) {
        tablinks[i].className = tablinks[i].className.replace(" active", "");
    }

    // Показать текущую вкладку и добавить класс "active" по ссылке, открывшей вкладку
    document.getElementById(this.value).style.display = "block";
    this.className += " active";
   // evt.currentTarget.className += " active";
}