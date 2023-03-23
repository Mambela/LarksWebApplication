/**
 * THIS FILE IS RESPONSIBLE FOR FILTERING THE DROPDOWNS FOR EACH SECTOR 
 * 
 *
 *  #COUNTRY
 *  #PROVINCES
 *  #MUNICIPALITY
 *  #NEIGHBOURHOOD
 *  #STREET 
 * 
 * ***/


$(document).ready(function () {


    //DIFFERENT SECTORS 
    const countryDropDown = document.getElementById('CountryId');
    const provincesDropDown = document.getElementById('provinceId');
    const municipalityDropDown = document.getElementById('municipalityId');
    const neighbourhoodDropDown = document.getElementById('neighbourhoodId');
    const streetDropDown = document.getElementById('streetId');






    if (countryDropDown) {

        countryDropDown.addEventListener('change', function handleChange(event) {
           
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;


            previewCountry.innerText = $("#countryId option:selected").text();

        });
    }


    if (provincesDropDown) {

        provincesDropDown.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;


            previewCountry.innerText = $("#countryId option:selected").text();

        });
    }




    if (municipalityDropDown) {

        municipalityDropDown.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;


            previewCountry.innerText = $("#countryId option:selected").text();

        });
    }



    if (neighbourhoodDropDown) {

        neighbourhoodDropDown.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;


            previewCountry.innerText = $("#countryId option:selected").text();

        });
    }



    if (streetDropDown) {

        streetDropDown.addEventListener('change', function handleChange(event) {
            ResetDropdowns();
            var countryId = event.target.value;
            RetrieveProvinceChildren('api/province/CountryProvinces?CountryId=' + countryId, provinceDropDownList);
            addressCountryid = countryId;


            previewCountry.innerText = $("#countryId option:selected").text();

        });
    }

    //METHODS TO POPULATE THE DROPDOWNS

    function RetrieveProvinceChildren(endpoint, destinationDropDownList) {
        $.ajax(
            {
                type: 'GET',
                dataType: 'JSON',
                url: BASEURL + endpoint,
                success:
                    function (response) {

                        currentProvinceList = response;
                        UpdateDropDownList(response, destinationDropDownList, 'province');

                        //UpdateProvinceChildren(response);
                    },
                error:
                    function (response) {
                        alert("Error " + response);
                    }
            });

    }


    function UpdateDropDownList(data, destinationControl, attribute = 'Neighbourhood') {



        var optionList = '<option disabled selected>Select a ' + attribute + ' </option>';

        for (var i = 0; i < data.length; i++) {

            optionList += "<option value=" + data[i].id + ">" + data[i].designation + "</option>";

        }

        destinationControl.innerHTML = optionList;




        if (attribute == 'Neighbourhood') {

            CreateNeighbourhoodCheckBoxes(data);
        }
    }


    function ResetDropdowns() {

        if (provincesDropDown) {
            provincesDropDown.innerHTML = '<option disabled selected> No Provinces </option>';
        }


        if (streetDropDown) {
            streetDropDown.innerHTML = '<option disabled selected> No Streets </option>';
        }

        if (districtDropDownList) {
            districtDropDownList.innerHTML = '<option disabled selected> No Districts </option>';
        }

        if (municipalityDropDown) {
            municipalityDropDown.innerHTML = '<option disabled selected> No Municipalities </option>';
        }



        if (neighbourhoodDropDown) {
            neighbourhoodDropDown.innerHTML = '<option disabled selected> No Neighbourhoods </option>';
        }
    }
});