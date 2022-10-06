

$(document).ready(function () {


    var hoodId = 0;
    const neighbourhoodDropDowm = document.getElementById('street_hoodId');
    const streetTable = document.getElementById('locationTable');



    var listLocation = function () {

        //var tableObj =
        $('#locationTable').DataTable({
            "destroy": true,
            "processing": true,
            "serverside": true,
            "filter": true,
            "ajax": {
                "url": BASEURL + "api/Location/LocationsInNeighbourhoodDT?neighbourhoodId=" + hoodId,
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



    if (neighbourhoodDropDowm) {
        neighbourhoodDropDowm.addEventListener('change', function handleChange(event) {

            hoodId = event.target.value;

            if (streetTable) {

                listLocation();
            }

     
        });
    }

});