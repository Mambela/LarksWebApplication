
$(document).ready(function () {

    /**
     * CASCADING DROPDOWNS: Country -> Province -> Municipality -> Suburb
     *
     * **/

    const countryDropDown = document.getElementById('CountryId');
    const provinceDropDown = document.getElementById('street_ProvinceId');
    const municipalityDropDown = document.getElementById('street_MunicipalityId');
    const suburbDropDown = document.getElementById('street_suburbId');
    const streetTable = document.getElementById('streetTable');

    function fillSelect(select, items, placeholder) {
        select.innerHTML = '<option value="">' + placeholder + '</option>' +
            items.map(function (item) {
                return '<option value="' + item.id + '">' + item.designation + '</option>';
            }).join('');
        select.disabled = items.length === 0;
    }

    function resetSelects(selects, placeholder) {
        selects.forEach(function (select) {
            if (!select) return;
            select.innerHTML = '<option value="">' + placeholder + '</option>';
            select.disabled = true;
        });
    }

    if (countryDropDown) {
        countryDropDown.addEventListener('change', function (event) {
            var countryId = event.target.value;

            resetSelects([municipalityDropDown, suburbDropDown], 'Select a province first');

            if (!countryId) {
                resetSelects([provinceDropDown], 'Select a country first');
                return;
            }

            resetSelects([provinceDropDown], 'Loading...');

            fetch(BASEURL + 'api/Province/CountryProvinces?CountryId=' + countryId)
                .then(function (r) { return r.json(); })
                .then(function (provinces) {
                    fillSelect(provinceDropDown, provinces, 'Select Province');
                })
                .catch(function (err) {
                    console.log('Failed to load provinces for country', err);
                });
        });
    }

    if (provinceDropDown) {
        provinceDropDown.addEventListener('change', function (event) {
            var provinceId = event.target.value;

            resetSelects([suburbDropDown], 'Select a municipality first');

            if (!provinceId) {
                resetSelects([municipalityDropDown], 'Select a province first');
                return;
            }

            resetSelects([municipalityDropDown], 'Loading...');

            fetch(BASEURL + 'api/Municipality/MunicipalitiesInProvince?provinceId=' + provinceId)
                .then(function (r) { return r.json(); })
                .then(function (municipalities) {
                    fillSelect(municipalityDropDown, municipalities, 'Select Municipality');
                })
                .catch(function (err) {
                    console.log('Failed to load municipalities for province', err);
                });
        });
    }

    if (municipalityDropDown) {
        municipalityDropDown.addEventListener('change', function (event) {
            var municipalityId = event.target.value;

            if (!municipalityId) {
                resetSelects([suburbDropDown], 'Select a municipality first');
                return;
            }

            resetSelects([suburbDropDown], 'Loading...');

            fetch(BASEURL + 'api/Suburb/sectorSuburb?sector=municipality&id=' + municipalityId)
                .then(function (r) { return r.json(); })
                .then(function (suburbs) {
                    fillSelect(suburbDropDown, suburbs || [], 'Select Suburb');
                })
                .catch(function (err) {
                    console.log('Failed to load suburbs for municipality', err);
                });
        });
    }

    /**
     * DATA TABLES FOR STREETS
     *
     * ***/

    var suburbId = 0;

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
                { "data": "designation", "name": "Street", "autoWidth": true },
                {
                    "render": function (data, row) {
                        return "<a href='" + BASEURL + "Street/Edit/" + row.id + "' class='btn btn-primary btn-sm'>Edit</a> " +
                            "<a href='" + BASEURL + "Street/Delete/" + row.id + "' class='btn btn-danger btn-sm'>Delete</a>";
                    }
                },
            ]
        });
    }

    if (suburbDropDown) {
        suburbDropDown.addEventListener('change', function handleChange(event) {

            suburbId = event.target.value;

            if (streetTable) {
                listStreets();
            }

        });
    }

});
