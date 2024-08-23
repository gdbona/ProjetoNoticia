$(function () {
    $.ajax({
        url: '<%=ResolveUrl( na.aspx/GetCoordenadas")%>',
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            var dados = JSON.parse(data.d);
            var latitude = dados.latitude;
            var longitude = dados.longitude;
            console.log(dados);
        },
        error: function (response) {
            alert(response.responseText);
        },
        failure: function (response) {
            alert(response.responseText);
        }
    });
});