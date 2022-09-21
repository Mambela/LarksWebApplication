

 


function RetrieveChildren(endpoint) {
    $.ajax(
        {
            type: 'GET',
            dataType: 'JSON',
            url: BASEURL + endpoint,
            success:
                function (response) {

                    currentProvinceList = response;

                    console.log(response);

                    return response;

                    //UpdateProvinceChildren(response);
                },
            error:
                function (response) {
                    alert("Error " + response);
                }
        });
}





function PostRequest(streetDTO, _endPoint)
{

    $.ajax({
        type: "POST",
        url: _endPoint,
        data: JSON.stringify(streetDTO),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

            swal("Save Address", "Address created successfully", "success");
        },
        failure: function (response) {
            swal("Save Address", "Address not saved!", "warning");
        },
        error: function (response) {

            swal("Save Address", "Error occurred while trying to save the address!", "error");
        }
    });

}


function RetrieveChildrenAsync(endpoint) {
    return new Promise((resolve, reject) => {
        $.ajax(
            {
                type: 'GET',
                dataType: 'JSON',
                url: BASEURL + endpoint,
                success:
                    function (response) {
                        
                        resolve(response);
                    },
                error:
                    function (response) {
                        reject(response);
                    }
            });

    });

}