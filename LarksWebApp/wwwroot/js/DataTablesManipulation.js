
$(document).ready(function () {

    /**
     * DATA TABLE FOR NEIGHBOURHOOD
     * 
     * **/

    var hoodId = 0;
    var listHoods = function () {

        //var tableObj =
        $('#neighbouhrhoodTable').DataTable({
            "destroy": true,
            "processing": true,
            "serverside": true,
            "filter": true,
            "ajax": {
                "url": 
 + "api/Hood/AddressesInNeighbourhood?neighbourhoodId=" + hoodId,
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
                { "data": "province", "name": "Province", "autoWidth": true },
                { "data": "municipality", "name": "Municipality", "autoWidth": true },
                { "data": "neighbourhood", "name": "Neighbourhood", "autoWidth": true },
                { "data": "streetName", "name": "Street", "autoWidth": true },
                { "data": "fullAddress", "name": "Address", "autoWidth": true },
                {
                    "render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
                },
            ]
        });
    }

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
                { "data": "desigantion", "name": "Street", "autoWidth": true },
                {
                    "render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=DeleteCustomer('" + row.id + "'); >Delete</a>"; }
                },
            ]
        });
    }
    
    //listHoods();
    const streetDropDown = document.getElementById('streetId');
    const neighbourhoodDropDowm = document.getElementById('street_hoodId');
    const neighbourhoodTable = document.getElementById('neighbouhrhoodTable');
    const streetTable = document.getElementById('streetTable');



    if (neighbourhoodDropDowm) {
        neighbourhoodDropDowm.addEventListener('change', function handleChange(event) {

            hoodId = event.target.value;

            if (neighbourhoodTable) {

                listHoods();
            }

            if (streetTable) {
                listStreets();
            }           

        });
    }

    /**
     * DATA TABLES FOR STREETS 
     * 
     * ***/
  



    function DeleteCustomer(id)
    {

    }
});
