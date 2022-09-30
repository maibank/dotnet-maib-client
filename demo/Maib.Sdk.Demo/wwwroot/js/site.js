const payButton = $("#maib_pay");

$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

let intervalId = undefined;
let transactionId = undefined;

payButton.on("click", function () {
    const rawData = $("#maib_payment_method_form").serializeArray();
    const submitData = {};

    $.each(rawData, (i, item) => {
        submitData[item.name] = item.value;
    });

    if (!submitData.amount || submitData.amount <= 0) {
        alert("Amount must be greater than 0.");
        return;
    }

    payButton.prop('disabled', true);

    $.ajax('/api/maib-payment/register-sms-transaction', {
        type: 'POST',
        data: submitData,
        dataType: 'json',
        success: function (data, status, xhr) {
            if (!data.success) {
                alert("Error: " + data.error);
                payButton.prop('disabled', false);
            } else {
                transactionId = data.transactionId;

                window.open(data.redirectUrl, '_blank');

                intervalId = window.setInterval(function () {
                    CheckTransaction();
                }, 3000);
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            alert("Request error: " + errorMessage + " " + textStatus);
            payButton.prop('disabled', false);
        }
    });
});

function CheckTransaction() {
    $.ajax('/api/maib-payment/transaction-result?transactionId=' + transactionId, {
        type: 'GET',
        dataType: 'json',
        success: function (data, status, xhr) {
            if (!data.success) {
                alert("Error: " + data.error);
            } else {
                if (data.result == 1) {
                    alert("Paid successfully with card: " + data.cardNumber);
                    clearInterval(intervalId);
                    payButton.prop('disabled', false);
                }
            }
        },
        error: function (jqXhr, textStatus, errorMessage) {
            alert("Request error: " + errorMessage + " " + textStatus);
        }
    });
}