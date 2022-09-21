
/*alert("Test JS");*/

//const { error } = require("jquery");


$(document).ready(function () {


    const BASEURL = "https://localhost:44339/";
    const selectedCityId = document.getElementById('CityId');
    const cityName = document.getElementById('newCityId');

 

    if (selectedCityId) {

        selectedCityId.addEventListener('change', function handleChange(event) {
           
            cityName.value = event.target.value;
        });
    }

    //DistrictId


    const selectedMunicipalityId = document.getElementById('MunicipalityId');
    const municipalityName = document.getElementById('newMunicipalityId');

    if (selectedMunicipalityId) {
        selectedMunicipalityId.addEventListener('change', function handleChange(event) {
           
            municipalityName.value = event.target.value;

          
        });
    }


    const selectedDistrictId = document.getElementById('DistrictId');
    const districtName = document.getElementById('newDistrictId');



    if (selectedDistrictId) {

        selectedDistrictId.addEventListener('change', function handleChange(event) {
            
            districtName.value = event.target.value;
             
        });
    }




    /*
     * AJAX CALLS TO POPULATE DROPDOWNS 
     */
    //Country details
    const selectedCountryId = document.getElementById('CountryId');


    const provinceDropDownList = document.getElementById('street_ProvinceId');


    const cityDropDownList = document.getElementById('street_cityId');

    const districtDropDownList = document.getElementById('street_districtId');

    const municipalityDropDownList = document.getElementById('street_MunicipalityId');

    const hoodDropDownList = document.getElementById('street_hoodId');

   // const btnSaveNeighbourhood = document.getElementById('saveNeighbourhood');

    
 
    /*
     * Declared variables 
     * **/

    var currentProvinceList;
    var currentNeighburhoodList= [];
    var coordinatesArray = [];

    /*
     * Fetch Provinces under country
     * **
     * Fetch the Negihbourhood under a municipality
     * Fetch the Negihbourhood under a District
     */
 
    if (selectedCountryId) {

        selectedCountryId.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList)
            
        });
    }

    if (provinceDropDownList) {

        provinceDropDownList.addEventListener('change',function handleChange(event) {

            var provinceId = event.target.value;

            singleProvince = $.grep(currentProvinceList, function (n, i) {
                return n.id == provinceId;
            });

            UpdateProvinceChildren(singleProvince);

        });

    }
  
    if (municipalityDropDownList)
    {

        municipalityDropDownList.addEventListener('change', function handleChange(event) {

            var municipalityId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=municipality&id=' + municipalityId, hoodDropDownList)

        });

    }

    if (districtDropDownList)
    {
        districtDropDownList.addEventListener('change', function handleChange(event) {

            var districtId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=districts&id=' + districtId, hoodDropDownList)

        });
    }

    if (cityDropDownList) {
        cityDropDownList.addEventListener('change', function handleChange(event) {

            var cityId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=city&id=' + cityId, hoodDropDownList)

        });
    }
      


    function RetrieveProvinceChildren(endpoint,provinceDropDownList)
    {
        $.ajax(
            {
                type: 'GET',
                dataType: 'JSON',
                url: BASEURL + endpoint,
                success:
                    function (response) {
  
                        currentProvinceList = response;
                        UpdateDropDownList(response, provinceDropDownList, 'province');
                       
                        //UpdateProvinceChildren(response);
                    },
                error:
                    function (response) {
                        alert("Error " + response);
                    }
            });

    }

    function RetrieveElementChildren(endpoint,destinationElement) {

        $.ajax(
            {
                type: 'GET',
                dataType: 'JSON',
                url: BASEURL+endpoint,
                success:
                    function (response) {

                        UpdateDropDownList(response, destinationElement);
                        
                    },
                error:
                    function (response)
                    {
                        alert("Error " + response);
                    }
        });
    }

    function UpdateDropDownList(data,destinationControl,attribute)
    {
       
        var optionList = '<option disabled selected>Select a ' + attribute +' </option>';

        for (var i = 0; i < data.length; i++) {

            optionList += "<option value=" + data[i].id + ">" + data[i].designation+"</option>";

        }

        destinationControl.innerHTML = optionList;

        if (!attribute) {
            
            CreateNeighbourhoodCheckBoxes(data);
        }

    }

    function UpdateProvinceChildren(data) {
        //update all children under a Province

        //update Municipalities
   

        UpdateDropDownList(data[0].municipalities, municipalityDropDownList,'municipality');
        UpdateDropDownList(data[0].districts, districtDropDownList,'district');
        UpdateDropDownList(data[0].cities, cityDropDownList, 'city');
 
    }

    function ResetDropdowns()
    {
        provinceDropDownList.innerHTML = '<option disabled selected> No Provinces </option>';

        cityDropDownList.innerHTML = '<option disabled selected> No Cities </option>';

        districtDropDownList.innerHTML = '<option disabled selected> No Districts </option>';

        municipalityDropDownList.innerHTML = '<option disabled selected> No Municipalities </option>';

        hoodDropDownList.innerHTML = '<option disabled selected> No Neighbourhoods </option>';
    }

    function CreateNeighbourhoodCheckBoxes(data)
    {
        var hoodDiv = document.getElementById('listOfNeighbourhoods');

        var listOfInputs = "";


        for (var i = 0; i < data.length; i++) {

            listOfInputs += `<input type="checkbox" class="btnNeighbourhoodCheckbox" value="${data[i].id}">
                            <label for="${data[i].id}"> ${data[i].designation}</label><br>`;
        }

        hoodDiv.innerHTML = listOfInputs;

        $('.btnNeighbourhoodCheckbox').click(function (event) {

            let neighbourhoodId =event.target.value;
          
            let checkedState = event.target.checked;


            if (checkedState) {
                //add element 
              
                currentNeighburhoodList.push(neighbourhoodId);
                
            }
            else
            {
                
                const index = currentNeighburhoodList.indexOf(neighbourhoodId);

                if (index > -1) {

                    currentNeighburhoodList.splice(index, 1);
                    
                }


                //remove element
            }
         

        });
    }

    //verify that value is in Array 
    function CheckNeighbourhood()
    {

    }

    /**
     * End of the provinces
     * **/

    //Working with coordinates and Street 

    
    //Save the details of a neighbourhood.
    $('#saveNeighbourhood').click(function () {
        alert('Test button');
        //create an object
        //Create an array of loc++
        //hoodDropDownList
        var streetDTO = new Object();
        var streetName = $("#Designation").val();

        streetDTO.Coordinates = coordinatesArray;
        streetDTO.NeighbourhoodID = currentNeighburhoodList;
        streetDTO.Designation = streetName;
        streetDTO.CityId
        streetDTO.MunicipalityId 

        if (streetDTO != null) {

            var endPoint = BASEURL + "api/Streets";

         

            $.ajax({
                type: "POST",
                url: endPoint,
                data: JSON.stringify(streetDTO),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert("Street created");
                },
                failure: function (response) {
                    alert("Failed to create Street");
                },
                error: function (response) {

                    alert("Error creating street");
                }
            });

        }
              
    });

    $('#addCoordinates').click(function () {
        
        var latitude = $('#latitude').val();
        var longitude = $('#longitude').val();

        var point = { lat: latitude, lng: longitude };
        
        
        addPointsCoordinatesToTable(point);
        coordinatesArray.push(point);
    });


    $("#coordinatesTable").on('click','.btnRemoveCoordinate', function () {

        var currentRow = $(this).closest("tr"); 

       $(this).closest('tr').remove();

        let latitude = currentRow.find("td:eq(0)").text(); 
        let longitude = currentRow.find("td:eq(1)").text();

        coordinatesArray = coordinatesArray.filter(function (o) {

            return ((o.lat != latitude) && (o.lng != longitude));
        });  

 
    });

    /*
     * Associate related neighbouhoods to street 
     *
     * **/
    $('#addNeighbourhood').click(function () {
      
        addNeighbourhoodToTable();
    });

    
    function addPointsCoordinatesToTable(point)
    {
   
        $('#tbodyCoordinates').append(`<tr>
                              <td>${point.lat}</td>
                              <td>${point.lng}</td>
                              <td><button type="button" class="btn btn-danger btn-sm btnRemoveCoordinate"> Remove </button></td>
                          </tr>`);

    }

    function addNeighbourhoodToTable() {

        if (!hoodDropDownList) {
            alert("Please select a neighbourhood");
        }
        else
        {
            var neighbourhoodId = hoodDropDownList.value;
            var neighbourhoodText = hoodDropDownList.innerText;

          
            hoodDropDownList = null;
        }


        //hoodDropDownList
    }

});


 

