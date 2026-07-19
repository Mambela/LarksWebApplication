
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
    var addressSuburbid = 0;
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

    const suburbDropDownList = document.getElementById('street_suburbId');

    const streetDropDownList = document.getElementById('streetId');

    const buildingTypeList = document.getElementById('BuildingTypeId');


    // const btnSaveSuburb = document.getElementById('saveSuburb');



    /*
     * Declared variables 
     * **/

    
    var currentSuburbList = [];
    var coordinatesArray = [];


    /*
     * Fetch Provinces under country
     * **
     * Fetch the Suburb under a municipality
     * Fetch the Suburb under a District
     */

    //

    if (selectedCountryId) {

        selectedCountryId.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;
                       
            if (previewCountry) {
                previewCountry.innerText = $("#CountryId option:selected").text();
            }

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

            if(previewProvince) {
             previewProvince.innerText = $("#street_ProvinceId  option:selected").text();
            }

        });

    }

    if (municipalityDropDownList) {

        municipalityDropDownList.addEventListener('change', function handleChange(event) {

            var municipalityId = event.target.value;

            RetrieveElementChildren('Suburb/sectorSuburb?sector=municipality&id=' + municipalityId, suburbDropDownList)
            addressMunicipalityid = municipalityId;

            if (previewMunicipality) {
            previewMunicipality.innerText = $("#street_MunicipalityId  option:selected").text();
            }

        });

    }

    if (districtDropDownList) {
        districtDropDownList.addEventListener('change', function handleChange(event) {

            var districtId = event.target.value;

            RetrieveElementChildren('Suburb/sectorSuburb?sector=districts&id=' + districtId, suburbDropDownList)
            addressDistrictid = districtId;

            if (previewDistrict) {

            previewDistrict.innerText = $("#street_districtId  option:selected").text();
            }

        });
    }

    if (cityDropDownList) {
        cityDropDownList.addEventListener('change', function handleChange(event) {

            var cityId = event.target.value;

            RetrieveElementChildren('Suburb/sectorSuburb?sector=city&id=' + cityId, suburbDropDownList)
            addressCityid = cityId;

            if (previewCity) {

            previewCity.innerText = $("#street_cityId  option:selected").text();
            }

        });
    }

    if (suburbDropDownList) {
        suburbDropDownList.addEventListener('change', function handleChange(event) {

            var suburbId = event.target.value;

            RetrieveElementChildren('Street/StreetsInSuburb?suburbId=' + suburbId, streetDropDownList)
            addressSuburbid = suburbId;


            if (previewSuburb) {
                previewSuburb.innerText = $("#street_suburbId  option:selected").text();
            }

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
                url: BASEURL+endpoint,
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

    function UpdateDropDownList(data, destinationControl, attribute ='Suburb') {

        

            var optionList = '<option disabled selected>Select a ' + attribute + ' </option>';

            for (var i = 0; i < data.length; i++) {

                optionList += "<option value=" + data[i].id + ">" + data[i].designation + "</option>";

            }

        if (destinationControl) {

        destinationControl.innerHTML = optionList;
        }

         
      

        if (attribute == 'Suburb') {

            CreateSuburbCheckBoxes(data);
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



        if (suburbDropDownList) {
            suburbDropDownList.innerHTML = '<option disabled selected> No Suburbs </option>';
        }
    }

    function CreateSuburbCheckBoxes(data) {
        var suburbDiv = document.getElementById('listOfSuburbs');

        if (suburbDiv) {
            var listOfInputs = "";


            for (var i = 0; i < data.length; i++) {

                listOfInputs += `<input type="checkbox" class="btnSuburbCheckbox" value="${data[i].id}">
                            <label for="${data[i].id}"> ${data[i].designation}</label><br>`;
            }

            suburbDiv.innerHTML = listOfInputs;

            console.log(suburbDiv);

            $('.btnSuburbCheckbox').click(function (event) {

                let suburbId = event.target.value;

                let checkedState = event.target.checked;


                if (checkedState) {
                    //add element 

                    currentSuburbList.push(suburbId);

                }
                else {

                    const index = currentSuburbList.indexOf(suburbId);

                    if (index > -1) {

                        currentSuburbList.splice(index, 1);

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


    //Save the details of a suburb.
    $('#saveSuburb').click(function () {

        //create an object
        //Create an array of loc++
        //suburbDropDownList
        var streetDTO = new Object();
        var streetName = $("#Designation").val();

        streetDTO.Coordinates = coordinatesArray;
        streetDTO.SuburbID = currentSuburbList;
        streetDTO.Designation = streetName;
        streetDTO.CityId
        streetDTO.MunicipalityId

        if (streetDTO != null) {

            var endPoint = BASEURL + "api/Streets";
            //var endPoint = "Street";



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

        var latitude =parseFloat($('#latitude').val());
        var longitude =parseFloat($('#longitude').val());

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
     * Associate related suburbs to street 
     *
     * **/
    $('#addSuburb').click(function () {

        addSuburbToTable();
    });


    function addPointsCoordinatesToTable(point) {

        $('#tbodyCoordinates').append(`<tr>
                              <td>${point.lat}</td>
                              <td>${point.lng}</td>
                              <td><button type="button" class="btn btn-danger btn-sm btnRemoveCoordinate"> Remove </button></td>
                          </tr>`);

    }

    function addSuburbToTable() {

        if (!suburbDropDownList) {
            alert("Please select a suburb");
        }
        else {
            var suburbId = suburbDropDownList.value;
            var suburbText = suburbDropDownList.innerText;


            suburbDropDownList = null;
        }


        //suburbDropDownList
    }



    /*
     * Address related Operations  * 
     * **/




    

    $('#createAddress').click(function () {

         
        var _lat = 0.0;
        var _lng = 0.0;



        _lat = $("#lat").val();
        _lng = $("#lng").val();
       

        var addressDTO = new Object();

        if (verifyAddress()) {

            if (!$("#lat").val())
            {
                _lat = 0.0;
            }
            if (!$("#lng").val())
            {
                _lng = 0.0;
            }

                var point = { lat: _lat, lng: _lng };

                coordinatesArray.push(point);

                addressDTO.CountryId = addressCountryid;
                addressDTO.ProvinceID = addressProvinceid;
                addressDTO.MunicipalityID = addressMunicipalityid;
                addressDTO.SuburbId = addressSuburbid;
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
    
    });

    //streetDropDownList
    

    /**
     * 
     * Declarations of preview elements
     * **/

    const previewStreetNumb = document.getElementById('streetNumbCurrentAddre');
    const previewStreetName = document.getElementById('streetNameCurrentAddre');
    const previewSuburb = document.getElementById('suburbCurrentAddre');
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

        if (!addressSuburbid) {

            swal("Error saving address", "Please select a suburb ", "error");
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




