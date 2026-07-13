
$(document).ready(function () {

    /**
     * CASCADING DROPDOWNS: Country -> Province -> Municipality -> Neighbourhood
     *
     * **/

    const countryDropDown = document.getElementById('CountryId');
    const provinceDropDown = document.getElementById('street_ProvinceId');
    const municipalityDropDown = document.getElementById('street_MunicipalityId');
    const neighbourhoodDropDowm = document.getElementById('street_hoodId');
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

            resetSelects([municipalityDropDown, neighbourhoodDropDowm], 'Select a province first');

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

            resetSelects([neighbourhoodDropDowm], 'Select a municipality first');

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
                resetSelects([neighbourhoodDropDowm], 'Select a municipality first');
                return;
            }

            resetSelects([neighbourhoodDropDowm], 'Loading...');

            fetch(BASEURL + 'api/Suburb/sectorNeighbourhood?sector=municipality&id=' + municipalityId)
                .then(function (r) { return r.json(); })
                .then(function (neighbourhoods) {
                    fillSelect(neighbourhoodDropDowm, neighbourhoods || [], 'Select Neighbourhood');
                })
                .catch(function (err) {
                    console.log('Failed to load neighbourhoods for municipality', err);
                });
        });
    }

    /**
     * DATA TABLES FOR STREETS
     *
     * ***/

    var hoodId = 0;

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

    if (neighbourhoodDropDowm) {
        neighbourhoodDropDowm.addEventListener('change', function handleChange(event) {

            hoodId = event.target.value;

            if (streetTable) {
                listStreets();
            }

        });
    }

});
