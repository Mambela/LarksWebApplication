
/*alert("Test JS");*/

//const { error } = require("jquery");


$(document).ready(function () {

    /*
     * Retrieve Items to save address
     * **/
    var addressCountryid = 0;
    var addressProvinceid = 0;
    var addressCityid = 0;
    var addressDistrictid = 0;
    var addressMunicipalityid = 0;
    var addressNeighbourhoodid = 0;
    var addressStreetId = 0;
    var addressBuildingTypeId = 0;
    var addressStreetNumber = "";
    var addressAdditionalInfo = "";
    var addressLat = "";
    var addressLng = "";
    //END



   /* const BASEURL = "https://localhost:44339/";*/
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

    const streetDropDownList = document.getElementById('streetId');

    const buildingTypeList = document.getElementById('BuildingTypeId');


    // const btnSaveNeighbourhood = document.getElementById('saveNeighbourhood');



    /*
     * Declared variables 
     * **/

    
    var currentNeighburhoodList = [];
    var coordinatesArray = [];


    /*
     * Fetch Provinces under country
     * **
     * Fetch the Negihbourhood under a municipality
     * Fetch the Negihbourhood under a District
     */

    //

    if (selectedCountryId) {

        selectedCountryId.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;
                       

            previewCountry.innerText = $("#CountryId option:selected").text();

        });
    }

    if (provinceDropDownList) {

        provinceDropDownList.addEventListener('change', function handleChange(event) {

            var provinceId = event.target.value;

            singleProvince = $.grep(currentProvinceList, function (n, i) {
                return n.id == provinceId;
            });

            UpdateProvinceChildren(singleProvince);
            addressProvinceid = provinceId;

            previewProvince.innerText = $("#street_ProvinceId  option:selected").text();

        });

    }

    if (municipalityDropDownList) {

        municipalityDropDownList.addEventListener('change', function handleChange(event) {

            var municipalityId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=municipality&id=' + municipalityId, hoodDropDownList)
            addressMunicipalityid = municipalityId;

            previewMunicipality.innerText = $("#street_MunicipalityId  option:selected").text();

        });

    }

    if (districtDropDownList) {
        districtDropDownList.addEventListener('change', function handleChange(event) {

            var districtId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=districts&id=' + districtId, hoodDropDownList)
            addressDistrictid = districtId;


            previewDistrict.innerText = $("#street_districtId  option:selected").text();

        });
    }

    if (cityDropDownList) {
        cityDropDownList.addEventListener('change', function handleChange(event) {

            var cityId = event.target.value;

            RetrieveElementChildren('api/Hood/sectorNeighbourhood?sector=city&id=' + cityId, hoodDropDownList)
            addressCityid = cityId;


            previewCity.innerText = $("#street_cityId  option:selected").text();

        });
    }

    if (hoodDropDownList) {
        hoodDropDownList.addEventListener('change', function handleChange(event) {

            var hoodId = event.target.value;

            RetrieveElementChildren('api/Streets/StreetsInNeighbourhood?neighbourhoodId=' + hoodId, streetDropDownList)
            addressNeighbourhoodid = hoodId;

            previewNeibourhood.innerText = $("#street_hoodId  option:selected").text();

        });
    }

    if (streetDropDownList)
    {
        streetDropDownList.addEventListener('change', function handleChange(event) {

            addressStreetId = event.target.value;

            previewStreetName.innerText = $("#streetId  option:selected").text();


        });
    }

    if (buildingTypeList)
    {
        buildingTypeList.addEventListener('change', function handleChange(event) {

            addressBuildingTypeId = event.target.value;
        });
    }






    function RetrieveProvinceChildren(endpoint, provinceDropDownList) {
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

    function RetrieveElementChildren(endpoint, destinationElement) {

        $.ajax(
            {
                type: 'GET',
                dataType: 'JSON',
                url: BASEURL + endpoint,
                success:
                    function (response) {

                        UpdateDropDownList(response, destinationElement);

                    },
                error:
                    function (response) {
                        alert("Error " + response);
                    }
            });
    }

    function UpdateDropDownList(data, destinationControl, attribute ='Neighbourhood') {

        

            var optionList = '<option disabled selected>Select a ' + attribute + ' </option>';

            for (var i = 0; i < data.length; i++) {

                optionList += "<option value=" + data[i].id + ">" + data[i].designation + "</option>";

            }

        destinationControl.innerHTML = optionList;

         
      

        if (attribute == 'Neighbourhood') {

            CreateNeighbourhoodCheckBoxes(data);
        }

    }

    function UpdateProvinceChildren(data) {
        //update all children under a Province

        //update Municipalities


        UpdateDropDownList(data[0].municipalities, municipalityDropDownList, 'municipality');
        UpdateDropDownList(data[0].districts, districtDropDownList, 'district');
        UpdateDropDownList(data[0].cities, cityDropDownList, 'city');

    }

    function ResetDropdowns() {

        if (provinceDropDownList) {
            provinceDropDownList.innerHTML = '<option disabled selected> No Provinces </option>';
        }


        if (cityDropDownList) {
            cityDropDownList.innerHTML = '<option disabled selected> No Cities </option>';
        }

        if (districtDropDownList) {
            districtDropDownList.innerHTML = '<option disabled selected> No Districts </option>';
        }

        if (municipalityDropDownList) {
            municipalityDropDownList.innerHTML = '<option disabled selected> No Municipalities </option>';
        }



        if (hoodDropDownList) {
            hoodDropDownList.innerHTML = '<option disabled selected> No Neighbourhoods </option>';
        }
    }

    function CreateNeighbourhoodCheckBoxes(data) {
        var hoodDiv = document.getElementById('listOfNeighbourhoods');

        if (hoodDiv) {
            var listOfInputs = "";


            for (var i = 0; i < data.length; i++) {

                listOfInputs += `<input type="checkbox" class="btnNeighbourhoodCheckbox" value="${data[i].id}">
                            <label for="${data[i].id}"> ${data[i].designation}</label><br>`;
            }

            hoodDiv.innerHTML = listOfInputs;

            console.log(hoodDiv);

            $('.btnNeighbourhoodCheckbox').click(function (event) {

                let neighbourhoodId = event.target.value;

                let checkedState = event.target.checked;


                if (checkedState) {
                    //add element 

                    currentNeighburhoodList.push(neighbourhoodId);

                }
                else {

                    const index = currentNeighburhoodList.indexOf(neighbourhoodId);

                    if (index > -1) {

                        currentNeighburhoodList.splice(index, 1);

                    }


                    //remove element
                }


            });
        }


    }


    /**
     * End of the provinces
     * **/

    //Working with coordinates and Street 


    //Save the details of a neighbourhood.
    $('#saveNeighbourhood').click(function () {

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


    $("#coordinatesTable").on('click', '.btnRemoveCoordinate', function () {

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


    function addPointsCoordinatesToTable(point) {

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
        else {
            var neighbourhoodId = hoodDropDownList.value;
            var neighbourhoodText = hoodDropDownList.innerText;


            hoodDropDownList = null;
        }


        //hoodDropDownList
    }



    /*
     * Address related Operations  * 
     * **/




    

    $('#createAddress').click(function () {

         
        var _lat = 0.0;
        var _lng = 0.0;



        _lat = $("#latitude").val();
        _lng = $("#longitude").val();
       

        var addressDTO = new Object();

        if (verifyAddress()) {

            if (!$("#latitude").val())
            {
                _lat = 0.0;
            }
            if (!$("#longitude").val())
            {
                _lng = 0.0;
            }

                var point = { lat: _lat, lng: _lng };

                coordinatesArray.push(point);

                addressDTO.CountryId = addressCountryid;
                addressDTO.ProvinceID = addressProvinceid;
                addressDTO.MunicipalityID = addressMunicipalityid;
                addressDTO.NeighbourhoodId = addressNeighbourhoodid;
                addressDTO.StreetID = addressStreetId;
                addressDTO.StreetNumber = $("#StreetNumber").val();
                addressDTO.AdditionalInfo = $("#AdditionalInfo").val();
                addressDTO.Coordinates = point;
                addressDTO.BuildingTypeId = addressBuildingTypeId;

        //addressDTO.BuildingTypeId =

            /* swal("Save Address", "Address created successfully", "success");*/

            console.log(addressDTO);

            PostRequest(addressDTO,BASEURL+"api/Address");
        }


        console.log(addressDTO);
        


    
    });

    //streetDropDownList
    

    /**
     * 
     * Declarations of preview elements
     * **/

    const previewStreetNumb = document.getElementById('streetNumbCurrentAddre');
    const previewStreetName = document.getElementById('streetNameCurrentAddre');
    const previewNeibourhood = document.getElementById('neighbourhoodCurrentAddre');
    const previewAdditionalCurrentInfo = document.getElementById('additionInfoCurrentAddre');
    const previewMunicipality = document.getElementById('municipalityCurrentAddre');
    const previewDistrict = document.getElementById('districtCurrentAddre');
    const previewCity = document.getElementById('cityCurrentAddre');
    const previewProvince = document.getElementById('provinceCurrentAddre');
    const previewCountry = document.getElementById('countryCurrentAddre');
    const previewLatCoordintes = document.getElementById('coordinateLatCurrentAddress');
    const previewLngCoordintes = document.getElementById('coordinateLngCurrentAddress');

    /*
     * Preview Elements.
     * **/

    var streetNumberEl = document.getElementById("StreetNumber");
    var additionalInfoEl = document.getElementById("AdditionalInfo");
    var LatEl = document.getElementById("latitude");
    var LngEl = document.getElementById("longitude");


    $('#StreetNumber').blur(function (event) {

        previewStreetNumb.innerText = $("#StreetNumber").val();
    });

    $('#AdditionalInfo').blur(function (event) {

        previewAdditionalCurrentInfo.innerText = $("#AdditionalInfo").val();
    });

    $('#latitude').blur(function (event) {

        previewLatCoordintes.innerText = $("#latitude").val();
    });

    $('#longitude').blur(function (event) {

        previewLngCoordintes.innerText = $("#longitude").val();
    });

    //$('#StreetNumber').blur(function (event) {

    //    previewCoordintes.innerText = $("#StreetNumber").val();
    //});

    function verifyAddress()
    {
        if (!addressCountryid) {

            swal("Error saving address","Please select a country ", "error");
            return false;
        }

        if (!addressProvinceid) {

            swal("Error saving address", "Please select a province ", "error");
            return false;
        }

        if (!addressMunicipalityid) {

            swal("Error saving address", "Please select a municipality ", "error");
            return false;
        }

        if (!addressNeighbourhoodid) {

            swal("Error saving address", "Please select a neighbourhood ", "error");
            return false;
        }


        if (!addressStreetId) {

            swal("Error saving address", "Please select a Street ", "error");
            return false;
        }
         


        if (!addressStreetId) {

            swal("Error saving address", "Please select a country ", "error");
            return false;
        }




        return true;
    }



});




