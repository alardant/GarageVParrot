$(document).ready(function () {
    $('#sendButton').click(function () {
        var formData = {
            firstName: $('#firstName').val(),
            lastName: $('#lastName').val(),
            email: $('#email').val(),
            phone: $('#phone').val(),
            subject: $('#subject').val(),
            message: $('#message').val()
        };

        $.ajax({
            type: 'POST',
            url: '/Contact/SendEmail',
            data: JSON.stringify(formData),
            contentType: 'application/json',
            success: function (result) {
                if (result.isSend === 'success') {
                    $('#resultMessage').html('<p style="color:#0D5B82">L\'email a été bien été envoyé!</p>');
                } else if (result.isSend === 'failed') {
                    $('#resultMessage').html('<p style="color:red">Échec de l\'envoi de l\'email, veuillez réessayer.</p>');
                }
            },
            error: function () {
                $('#resultMessage').html('<p style="color:red">Erreur lors de l\'envoi de la requête.</p>');
            }
        });
    });
});
