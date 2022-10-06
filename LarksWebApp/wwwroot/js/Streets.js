
$(document).ready(function () {

    /**
     * DATA TABLE FOR NEIGHBOURHOOD
     * 
     * **/

    var hoodId = 0;
    const neighbourhoodDropDowm = document.getElementById('street_hoodId');
    const streetTable = document.getElementById('streetTable');


    /**
     * DATA TABLES FOR STREETS 
     * 
     * ***/


    var listStreets = function () {

        $('#streetTable').DataTable({

            "destroy": true,
            "processing": true,
            "serverside": true,
            "filter": true,
            "ajax": {
                "url": BASEURL + "api/Streets/StreetsInNeighbourhood?neighbourhoodId=" + hoodId,
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
                { "data": "designation", "name": "Street", "autoWidth": true },
                {
                    "render": function (data, row) { return "<a href='#' class='btn btn-danger btn-sm' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
                },
            ]
        });
    }

    //listHoods();




    if (neighbourhoodDropDowm) {
        neighbourhoodDropDowm.addEventListener('change', function handleChange(event) {

            hoodId = event.target.value;

            if (streetTable) {
                listStreets();
            }

        });
    }





    function DeleteCustomer(id) {

    }
});
