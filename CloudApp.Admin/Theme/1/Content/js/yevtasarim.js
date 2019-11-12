
//Open link
function OpenLink(link)
{ window.open("/" + link, "_self") }


function MailControl(email) {
    var control = new RegExp(/^[^0-9][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[@][a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{2,4}$/i);
    return control.test(email);
}


function FeedBack()
{
    var name = $('#txtName').val();
    var phone = $('#txtPhone').val();
    var mail = $('#txtEmail').val();
    var subject = $('#txtKonu').val();
    var message = $('#txtMesaj').val();

    if (MailControl(mail) && name != "" && phone != "" && subject != "" && message != "") {
        $.ajax({
            type: 'POST',
            url: '/iletisim.aspx/FeedBack',
            data: '{ "name":"' + name + '", "phone":"' + phone + '", "mail":"' + mail + '", "subject":"' + subject + '", "message":"' + message + '"}',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (result) {
                $('#btnSend').hide();
                alert(result.d);
            },
            error: function () {
                alert(result.d);
            }
        });
    }
    else
        alert("Hatalı bilgi girişi yaptınız. Lütfen giriş yaptığınız bilgileri kontrol ediniz.");
      
}
