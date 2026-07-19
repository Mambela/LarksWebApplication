
$(document).ready(function () {

    /**
     * DATA TABLE FOR SUBURB
     * 
     * **/

    var suburbId = 0;
    var listSuburbs = function () {

        //var tableObj =
        $('#suburbTable').DataTable({
            "destroy": true,
            "processing": true,
            "serverside": true,
            "filter": true,
            "ajax": {
                "url": 
                    + "api/Suburb/AddressesInSuburb?suburbId=" + suburbId,
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
                { "data": "suburb", "name": "Suburb", "autoWidth": true },
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
                "url": BASEURL + "api/Streets/StreetsInSuburb?suburbId=" + suburbId,
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
    
    //listSuburbs();
    const streetDropDown = document.getElementById('streetId');
    const suburbDropDown = document.getElementById('street_suburbId');
    const suburbTable = document.getElementById('suburbTable');
    const streetTable = document.getElementById('streetTable');



    if (suburbDropDown) {
        suburbDropDown.addEventListener('change', function handleChange(event) {

            suburbId = event.target.value;

            if (suburbTable) {

                listSuburbs();
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
