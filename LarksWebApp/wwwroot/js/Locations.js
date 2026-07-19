

$(document).ready(function () {


    var suburbId = 0;
    const suburbDropDown = document.getElementById('street_suburbId');
    const streetTable = document.getElementById('locationTable');



    var listLocation = function () {

        //var tableObj =
        $('#locationTable').DataTable({
            "destroy": true,
            "processing": true,
            "serverside": true,
            "filter": true,
            "ajax": {
                "url": BASEURL + "api/Location/LocationsInSuburbDT?suburbId=" + suburbId,
                "type": "POST",
                "headers": {
                    "Content-Type": "application/x-www-form-urlencoded"
                }
                //***"datatype":"json"
            },
            "columnDefs": [{
                "targets": [0],
                "visible": false,
                "searchable": false
            }],
            "columns": [
                { "data": "id", "name": "Id", "autoWidth": true },
                { "data": "designation", "name": "Designation", "autoWidth": true },
                { "data": "category", "name": "Category", "autoWidth": true },
                { "data": "subcategory", "name": "Subcategory", "autoWidth": true },
                { "data": "streetName", "name": "Street", "autoWidth": true },
                { "data": "additionalInformation", "name": "Additional Information", "autoWidth": true },
                {
                    "render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
                },
            ]
        });
    }



    if (suburbDropDown) {
        suburbDropDown.addEventListener('change', function handleChange(event) {

            suburbId = event.target.value;

            if (streetTable) {

                listLocation();
            }

     
        });
    }

});