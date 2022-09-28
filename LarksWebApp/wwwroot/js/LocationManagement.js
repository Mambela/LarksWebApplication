
$(document).ready(function () {



    //$('#searchCountryId').on('input', function () {

    //    var shownVal = document.getElementById("searchCountryId").value;
    //    //var value2send = document.querySelector("#dataListOfCountry option[value='" + shownVal + "']").dataset.value;

    //    //alert(value2send);
    //    console.log(document.querySelector("#dataListOfCountry option[value='" + shownVal + "']"));
    //});

    var currentProvinceList;

    //const cityDropDownList = document.getElementById('street_cityId');

    const districtDropDownList = document.getElementById('dataListDistrict');

    const municipalityDropDownList = document.getElementById('dataListOfMunicipality');


    $('#searchCountryId').on('input', function (e) {


        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var countryId = $option.attr('data-value')
                $hiddenInput.val(countryId);


                console.log('Country Id ',countryId);

                FetchProvinces(countryId);
                break;
            }
        }
    });


    $('#searchProvince').on('input', function (e) {


        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var selectedProvinceId = $option.attr('data-value')
                $hiddenInput.val(selectedProvinceId);

                console.log('Province Id ', selectedProvinceId);
                PredefineProvince(selectedProvinceId);

                break;
            }
        }
    });


    $('#searchMunicipality').on('input', function (e) {


        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var selectedMunicipalityId = $option.attr('data-value')
                $hiddenInput.val(selectedMunicipalityId);


                var endPoint = "api/Hood/sectorNeighbourhood?sector=municipality&id=" + selectedMunicipalityId;

                console.log('Municipality Id ', selectedMunicipalityId);
                FetchChildNodes(endPoint, '#dataListNeighbourhood');

                 
                break;
            }
        }
    });


    $('#searchNeighbourhood').on('input', function (e) {
        //we are searching the streets under Neighbourhood.

        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        console.log(label); 

        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);

            let optionName = $option.text().trim();
            let labelName = label.trim();

            optionName = optionName.replaceAll(" ", "");
            labelName = labelName.replaceAll(" ","");

            if (optionName.trim() === labelName.trim()) {
                var selectedNeighbourhoodId = $option.attr('data-value')
                $hiddenInput.val(selectedNeighbourhoodId);


                var endPoint = "api/Streets/StreetsInNeighbourhood?neighbourhoodId=" + selectedNeighbourhoodId;
 
                console.log('Neighbourhood Id ', selectedNeighbourhoodId);

                FetchChildNodes(endPoint, '#dataListStreet');


                break;
            }
        }
    });


    $('#searchAddress').on('input', function (e) {
        //we are searching the streets under Neighbourhood.

        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        console.log(label); 

        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);

            let optionName = $option.text().trim();
            let labelName = label.trim();

            optionName = optionName.replaceAll(" ", "");
            labelName = labelName.replaceAll(" ","");

            if (optionName.trim() === labelName.trim()) {
                var selectedAddressId = $option.attr('data-value')
                $hiddenInput.val(selectedAddressId);


                break;
            }
        }
    });


    $('#searchSubcategoryId').on('input', function (e) {


        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var subCategoryId = $option.attr('data-value')
                $hiddenInput.val(subCategoryId);                                   
                
                break;
            }
        }
    });
    
    $('#searchCategoryId').on('input', function (e) {


        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();


        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var categoryId = $option.attr('data-value')
                $hiddenInput.val(categoryId);

                var endPoint = "api/subcategory/SubcategoriesInCategory?categoryId=" + categoryId;
                                
                FetchChildNodes(endPoint, '#dataListOfSubcategory');
                break;
            }
        }
    });


    $('#searchStreet').on('input', function (e) {
        //we are searching the addresas under Streets.

        var $input = $(e.target);
        $options = $('#' + $input.attr('list') + ' option'),
            $hiddenInput = $('#' + $input.attr('id') + '_hidden'),
            label = $input.val();

        console.log(label);

        $hiddenInput.val(label);

        for (var i = 0; i < $options.length; i++) {
            var $option = $options.eq(i);


            if ($option.text().trim() === label.trim()) {
                var selectedAddressId = $option.attr('data-value')
                $hiddenInput.val(selectedAddressId);


                console.log(selectedAddressId); 
                var endPoint = "api/Address/StreetAddress?streetId=" + selectedAddressId;

                console.log('Address Id', selectedAddressId);
                FetchChildNodes(endPoint, '#dataListAddress', "fulladdress");


                break;
            }
        }
    });

 


    function FetchProvinces(countryId) {
        var endPoint = 'api/province/CountryProvinces?CountryId=' + countryId;

        (async () => {
            try {
                let result_data = await RetrieveChildrenAsync(endPoint);

                $("#dataListNeighbourhood").empty();

                console.log(result_data);
                var listOfProvinces = result_data;
                currentProvinceList = result_data;

                console.log(listOfProvinces);

                UpdateChildrenData(listOfProvinces, "#dataListOfProvince");

            } catch (error) {

                console.log(error);
            }

        })();



        //console.log(listOfProvinces);
    }


    function FetchChildNodes(endPoint,targetElement,columnDecision="default")
    {
        (async () => {
            try {
                let result_data = await RetrieveChildrenAsync(endPoint);

                $(targetElement).empty();

                console.log(result_data);
                var listOfNeighbourhoods = result_data;
                //currentProvinceList = result_data;
                               

                    switch (columnDecision) {

                        case "default":
                            UpdateChildrenData(listOfNeighbourhoods, targetElement);
                            break;
                        case "fulladdress":
                            UpdateAddressData(listOfNeighbourhoods, targetElement);
                            break;
                        default:
                }


                console.log(listOfNeighbourhoods);


            } catch (error) {

                console.log(error);
            }

        })();
    }


    
    function UpdateChildrenData(data,TargetElement) {
        
        for (var i = 0; i < data.length; i++) {

            $(TargetElement).append("<option data-value='"+data[i].id+"'> "+data[i].designation+"</option>");
        }

    }

    function UpdateAddressData(data, TargetElement) {
        
        for (var i = 0; i < data.length; i++) {

            $(TargetElement).append("<option data-value='" + data[i].id + "'> " + data[i].fullAddress +"</option>");
        }

    }


    function PredefineProvince(provinceId)
    {
       var singleProvince = $.grep(currentProvinceList, function (n, i) {
            return n.id == provinceId;
       });

        console.log(singleProvince);

        UpdateProvinceChildren(singleProvince);
    }

    function UpdateDropDownList(data, destinationControl, attribute = 'Neighbourhood') {

        var optionList = '<option disabled selected>Select a ' + attribute + ' </option>';

        for (var i = 0; i < data.length; i++) {

            optionList += "<option data-value=" + data[i].id + ">" + data[i].designation + "</option>";

        }
        destinationControl.innerHTML = optionList;
    }


    function UpdateProvinceChildren(data) {
        //update all children under a Province

        //update Municipalities


        UpdateDropDownList(data[0].municipalities, municipalityDropDownList, 'municipality');
        UpdateDropDownList(data[0].districts, districtDropDownList, 'district');
        //UpdateDropDownList(data[0].cities, cityDropDownList, 'city');

    }



    //image management Section
    document.querySelector("#Files").addEventListener("change", (e) => { //CHANGE EVENT FOR UPLOADING PHOTOS
        if (window.File && window.FileReader && window.FileList && window.Blob) { //CHECK IF FILE API IS SUPPORTED
            const files = e.target.files; //FILE LIST OBJECT CONTAINING UPLOADED FILES
            const output = document.querySelector("#result");
            output.innerHTML = "";


            for (let i = 0; i < files.length; i++) { // LOOP THROUGH THE FILE LIST OBJECT
                if (!files[i].type.match("image")) continue; // ONLY PHOTOS (SKIP CURRENT ITERATION IF NOT A PHOTO)
                const picReader = new FileReader(); // RETRIEVE DATA URI 
                picReader.addEventListener("load", function (event) { // LOAD EVENT FOR DISPLAYING PHOTOS
                    const picFile = event.target;
                    let div = document.createElement("div");
                    div.classList = 'col-md-3';
                    div.innerHTML = `<img class="img-thumbnail" src="${picFile.result}" title="${picFile.name}"/>
                        `;
                    output.appendChild(div);
                });
                picReader.readAsDataURL(files[i]); //READ THE IMAGE
            }
        } else {
            alert("Your browser does not support File API");
        }
    });

});

