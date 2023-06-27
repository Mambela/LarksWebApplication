// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//const BASEURL = "https://larks.azurewebsites.net/";
//const BASEURL = "https://localhost:7119/";
const BASEURL = "https://localhost:44339/";

//Address

var createAddress = {
    "optionalId": "",
    "hookname": "",
    "responseId": "",
    "lat": "",
    "lng": ""
};
document.getElementById("createAddress").textContent = JSON.stringify(createAddress, null, 2);





//GET Locations by Category

var createLocationResponse = {
    "id": 12,
    "designation": "BMW Waterfall",
    "country": null,
    "countryId": 2,
    "listofCountryDTO": null,
    "municipalityID": 1,
    "suburbId": 5,
    "cityId": 3,
    "subCategories": [
        4,
        3
    ],
    "files": null,
    "additionalInfo": "BMW sellerlocated in front of mall of africa.",
    "categoryId": 1,
    "lat": -26.011366,
    "lng": 28.109463
}

document.getElementById("createLocationResponse").textContent = JSON.stringify(createLocationResponse, null, 2);


var getlocationsByCategoryResponse =
    [
        {
            "id": 9,
            "designation": "A corner in  Lever",
            "additionalInformation": "A random place opposite Sandridge.",
            "categoryId": 2,
            "category": "Industry",
            "subcategory": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 8

                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 9

                }
            ],
            "images": [
                "7f663084-d289-4ae4-9778-1d4de5817a57.PNG"
            ],
            "lat": -25.976280,
            "lng": 28.119166
        }
    ];

document.getElementById("getlocationsByCategoryResponse").textContent = JSON.stringify(getlocationsByCategoryResponse, null, 2);

var locationsByDistanceCategory = {
    "pageNumber": 1,
    "pageSize": 2,
    "firstPage": "https://localhost:44339/api/location/locationbydistanceandcategory?distance=1&category=2&lat=-25.973419&lng=28.119508&pageNumber=1&pageSize=2",
    "lastPage": "https://localhost:44339/api/location/locationbydistanceandcategory?distance=1&category=2&lat=-25.973419&lng=28.119508&pageNumber=3&pageSize=2",
    "totalPages": 3,
    "totalRecords": 5,
    "nextPage": "https://localhost:44339/api/location/locationbydistanceandcategory?distance=1&category=2&lat=-25.973419&lng=28.119508&pageNumber=2&pageSize=2",
    "previousPage": null,
    "data": [
        {
            "id": 3,
            "designation": "Carlswald",
            "additionalInformation": "Some place close to Sandridge in carlswald",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0.695597950283581,
            "lat": -25.978936,
            "lng": 28.116227,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "Speed test1.PNG"
            ]
        },
        {
            "id": 4,
            "designation": "A fine Circle",
            "additionalInformation": "Some place close to carlswald",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0.885508634450344,
            "lat": -25.978446,
            "lng": 28.112637,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "Speed test1.PNG"
            ]
        }
    ],
    "succeeded": true,
    "errors": null,
    "message": null
};

document.getElementById("locationsByDistanceCategory").textContent = JSON.stringify(locationsByDistanceCategory, null, 2);

var locationsByDistance = {
    "pageNumber": 3,
    "pageSize": 2,
    "firstPage": "https://localhost:44339/api/location/locationbydistance?distance=1&lat=-25.973419&lng=28.119508&pageNumber=1&pageSize=2",
    "lastPage": "https://localhost:44339/api/location/locationbydistance?distance=1&lat=-25.973419&lng=28.119508&pageNumber=3&pageSize=2",
    "totalPages": 3,
    "totalRecords": 6,
    "nextPage": null,
    "previousPage": "https://localhost:44339/api/location/locationbydistance?distance=1&lat=-25.973419&lng=28.119508&pageNumber=2&pageSize=2",
    "data": [
        {
            "id": 10,
            "designation": "Mercedez Midrand",
            "additionalInformation": "Mercedez benz main seller in Midrand. ",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0.52755942626975,
            "lat": -25.978129,
            "lng": 28.118871,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "05473a99-acc5-497e-ae70-615164690e12.jpg"
            ]
        },
        {
            "id": 11,
            "designation": "Mercedez Midrand",
            "additionalInformation": "Mercedez benz main seller in Midrand. ",
            "category": "Business",
            "categoryId": 1,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0.245235899549542,
            "lat": -25.973972,
            "lng": 28.117133,
            "subcategories": [
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                },
                {
                    "id": 4,
                    "designation": "Offices",
                    "categoryId": 0,
                    "lng": "en"
                }
            ],
            "images": [
                "20eefc2a-558b-4155-981a-531fb1c0d7c4.jpg"
            ]
        }
    ],
    "succeeded": true,
    "errors": null,
    "message": null
};

document.getElementById("locationsByDistance").textContent = JSON.stringify(locationsByDistance, null, 2);


var locationsInCity = {
    "pageNumber": 2,
    "pageSize": 3,
    "firstPage": "https://localhost:44339/api/location/locationsinmunicipality?municipalityId=1&pageNumber=1&pageSize=3",
    "lastPage": "https://localhost:44339/api/location/locationsinmunicipality?municipalityId=1&pageNumber=2&pageSize=3",
    "totalPages": 2,
    "totalRecords": 6,
    "nextPage": null,
    "previousPage": "https://localhost:44339/api/location/locationsinmunicipality?municipalityId=1&pageNumber=1&pageSize=3",
    "data": [
        {
            "id": 9,
            "designation": "A corner in  Lever",
            "additionalInformation": "A random place opposite Sandridge.",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.97628,
            "lng": 28.119166,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "7f663084-d289-4ae4-9778-1d4de5817a57.PNG"
            ]
        },
        {
            "id": 10,
            "designation": "Mercedez Midrand",
            "additionalInformation": "Mercedez benz main seller in Midrand. ",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.978129,
            "lng": 28.118871,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "05473a99-acc5-497e-ae70-615164690e12.jpg"
            ]
        },
        {
            "id": 11,
            "designation": "Mercedez Midrand",
            "additionalInformation": "Mercedez benz main seller in Midrand. ",
            "category": "Business",
            "categoryId": 1,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.973972,
            "lng": 28.117133,
            "subcategories": [
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                },
                {
                    "id": 4,
                    "designation": "Offices",
                    "categoryId": 0,
                    "lng": "en"
                }
            ],
            "images": [
                "20eefc2a-558b-4155-981a-531fb1c0d7c4.jpg"
            ]
        }
    ],
    "succeeded": true,
    "errors": null,
    "message": null
};

document.getElementById("locationsInCity").textContent = JSON.stringify(locationsInCity, null, 2);


var locationmunicipality = {
    "pageNumber": 1,
    "pageSize": 4,
    "firstPage": "https://localhost:44339/api/location/locationsincity?cityId=3&pageNumber=1&pageSize=4",
    "lastPage": "https://localhost:44339/api/location/locationsincity?cityId=3&pageNumber=2&pageSize=4",
    "totalPages": 2,
    "totalRecords": 6,
    "nextPage": "https://localhost:44339/api/location/locationsincity?cityId=3&pageNumber=2&pageSize=4",
    "previousPage": null,
    "data": [
        {
            "id": 3,
            "designation": "Carlswald",
            "additionalInformation": "Some place close to Sandridge in carlswald",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.978936,
            "lng": 28.116227,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "Speed test1.PNG"
            ]
        },
        {
            "id": 4,
            "designation": "A fine Circle",
            "additionalInformation": "Some place close to carlswald",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.978446,
            "lng": 28.112637,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "Speed test1.PNG"
            ]
        },
        {
            "id": 8,
            "designation": "Midauto",
            "additionalInformation": "A place where you can fix your car and at the same time get drinks with your friends. ",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.977531,
            "lng": 28.115977,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "957b8764-bc2e-4380-a9f5-d52bbeaadc15.PNG"
            ]
        },
        {
            "id": 9,
            "designation": "A corner in  Lever",
            "additionalInformation": "A random place opposite Sandridge.",
            "category": "Industry",
            "categoryId": 2,
            "municipality": "City of Johannesburg Metropolitan Municipality",
            "suburb": "Carlswald",
            "city": "Midrand",
            "distance": 0,
            "lat": -25.97628,
            "lng": 28.119166,
            "subcategories": [
                {
                    "id": 2,
                    "designation": "Supermarket",
                    "categoryId": 0,
                    "lng": "en"
                },
                {
                    "id": 3,
                    "designation": "Assembleia",
                    "categoryId": 0,
                    "lng": "pt"
                }
            ],
            "images": [
                "7f663084-d289-4ae4-9778-1d4de5817a57.PNG"
            ]
        }
    ],
    "succeeded": true,
    "errors": null,
    "message": null
};

document.getElementById("locationmunicipality").textContent = JSON.stringify(locationmunicipality, null, 2);


var createAddressByDemographic = {
    "osmtype": "way",
    "osm_id": 60334883,
    "lat": "-8.83794705",
    "lon": "13.234773948986573",
    "displayname": "Hotel Forum, Travessa Ho Chi Mimh, Martires de Kifangondo, Distrito Urbano da Maianga, Luanda, Município de Luanda, Luanda, Angola",
    "address": {
        "tourism": "Hotel Forum",
        "road": "Travessa Ho Chi Mimh",
        "suburb": "Martires de Kifangondo",
        "city": "Luanda",
        "county": "Município de Luanda",
        "state": "Luanda",
        "ISO31662lvl4": "AO-LUA",
        "country": "Angola",
        "countrycode": "ao"
    }
};

document.getElementById("createAddressByDemographic").textContent = JSON.stringify(createAddressByDemographic,null,2)

var updateCategory = {
    "id": 1,
    "designation": "Parks",
    "lng": "en"
};

document.getElementById("updateCategory").textContent = JSON.stringify(updateCategory, null, 2);


var getAllCategories = [
    {
        "id": 6,
        "designation": "Mercado",
        "lng": "pt"
    },
    {
        "id": 9,
        "designation": "Governo",
        "lng": "pt"
    }
];

document.getElementById("getAllCategories").textContent = JSON.stringify(getAllCategories, null, 2);




var allcountries = [
    {
        "id": 123,
        "designation": "Angola",
    },
    {
        "id": 564,
        "designation": "République démocratique du Congo",
    },
    {
        "id": 452,
        "designation": "South Africa",
    }
];

document.getElementById("allcountries").textContent = JSON.stringify(allcountries, null, 2);


var countriesByName = {

        "id": 2,
        "designation": "South Africa",
        "provinces": [

            {
                "id": 2,
                "designation": "Gauteng",
                "countryId": 2
            },
            {
                "id": 4,
                "designation": "Western Cape",
                "countryId": 2
            },
            {
                "id": 5,
                "designation": "Eastern Cape",
                "countryId": 2
            }

        ]

    };


document.getElementById("countriesByName").textContent = JSON.stringify(countriesByName, null, 2);


var countriesById = {
    "id": 123,
    "designation": "République démocratique du Congo",
    "provinces": [
        {
            "id": 1,
            "designation": "Kinshasa",
            "countryId": 1231,
            
            "districts": [
                {
                    "id": 12231,
                    "designation": "Gombe",
                    "provinceId": 11
                }
            ],
            "cities": [
                {
                    "provinceId": 11,
                    "id": 112312,
                    "designation": "Kinshasa"
                }
            ]
        }
    ]
};


document.getElementById("countrybyId").textContent = JSON.stringify(countriesById, null, 2);



/**
 * City Related Opeartions
 * 
 */

var createCity = {
    "provinceId": "2",
    "designation": "Kilamba"
};

document.getElementById("createCity").textContent = JSON.stringify(createCity, null, 2);

var cityById = {
    "provinceId": 2,
    "id": 14,
    "designation": "TestCity"
};

document.getElementById("cityById").textContent = JSON.stringify(cityById, null, 2);

var citiesinaprovince = [
    {
        "provinceId": 2,
        "id": 2,
        "designation": "Pretoria"
    },
    {
        "provinceId": 2,
        "id": 3,
        "designation": "Midrand"
    },
    {
        "provinceId": 2,
        "id": 4,
        "designation": "Randburg"
    },
    {
        "provinceId": 2,
        "id": 5,
        "designation": "Roodepoort"
    }
];

document.getElementById("citiesinaprovince").textContent = JSON.stringify(citiesinaprovince, null, 2);


var citiesinacountry = [
    {
        "provinceId": 3,
        "id": 11,
        "designation": "Cacuaco"
    },
    {
        "provinceId": 3,
        "id": 18,
        "designation": "Luanda"
    },
    {
        "provinceId": 6,
        "id": 15,
        "designation": "M'banza Congo"
    },
    {
        "provinceId": 7,
        "id": 16,
        "designation": "Lobito"
    },
    {
        "provinceId": 8,
        "id": 17,
        "designation": "Lubango"
    }
];

document.getElementById("citiesinacountry").textContent = JSON.stringify(citiesinacountry, null, 2);

/**
 * District Related operations
 */

var createDistrict = {
    "designation": "Barumbu",
    "provinceId": "1"
};

document.getElementById("createDistrict").textContent = JSON.stringify(createDistrict, null, 2);


var getSingleDistrict = {
    "id": 2,
    "designation": "Barumbu",
    "provinceId": 1
};

document.getElementById("getSingleDistrict").textContent = JSON.stringify(getSingleDistrict, null, 2);


var getprovincedistricts = [
    {
        "id": 1,
        "designation": "Gombe",
        "provinceId": 1
    },
    {
        "id": 2,
        "designation": "Barumbu",
        "provinceId": 1
    }
];

document.getElementById("getprovincedistricts").textContent = JSON.stringify(getprovincedistricts, null, 2);

 



/**
 * Province Operations
 */

var provinceCreate = {
    "countryId": "2",
    "designation": "Province Name"
};

document.getElementById("provinceCreate").textContent = JSON.stringify(provinceCreate, null, 2);


var provincesInCountrybyName = [
    {
        "id": 3,
        "designation": "Luanda",
        "countryId": 3,

    },
    {
        "id": 6,
        "designation": "Zaire",
        "countryId": 3,
    },
    {
        "id": 7,
        "designation": "Benguela",
        "countryId": 3,

    },
    {
        "id": 8,
        "designation": "Huíla",
        "countryId": 3,

    }
];


document.getElementById("provincesInCountrybyName").textContent = JSON.stringify(provincesInCountrybyName, null, 2);


var provincesInCountry = [
    {
        "id": 3,
        "designation": "Luanda",
        "countryId": 3,
        "municipalities": [
            {
                "id": 4,
                "designation": "Município do Cacuaco",
                "provinceId": 3
            },
            {
                "id": 13,
                "designation": "Município do Belas",
                "provinceId": 3
            },
            {
                "id": 14,
                "designation": "Município de Luanda",
                "provinceId": 3
            }
        ],
        
        "cities": [
            {
                "provinceId": 3,
                "id": 11,
                "designation": "Cacuaco"
            },
            {
                "provinceId": 3,
                "id": 18,
                "designation": "Luanda"
            }
        ]
    },
    {
        "id": 6,
        "designation": "Zaire",
        "countryId": 3,
      
        "cities": [
            {
                "provinceId": 6,
                "id": 15,
                "designation": "M'banza Congo"
            }
        ]
    },
    {
        "id": 7,
        "designation": "Benguela",
        "countryId": 3,
        "municipalities": [
            {
                "id": 12,
                "designation": "Catumbela",
                "provinceId": 7
            }
        ],
      
        "cities": [
            {
                "provinceId": 7,
                "id": 16,
                "designation": "Lobito"
            }
        ]
    },
    {
        "id": 8,
        "designation": "Huíla",
        "countryId": 3,
        "cities": [
            {
                "provinceId": 8,
                "id": 17,
                "designation": "Lubango"
            }
        ]
    }
];

document.getElementById("provincesInCountry").textContent = JSON.stringify(provincesInCountry, null, 2);


/**
 * Municipalities opeations
 */

var createMunicipalityObj = {
    "designation": "Município de Luanda",
    "provinceid": "1"
};

document.getElementById("createMunicipalityObj").textContent = JSON.stringify(createMunicipalityObj, null, 2);


var municipalitiesInProvince = [
    {
        "id": 4,
        "designation": "Município do Cacuaco",
        "provinceId": 3,
      
    },
    {
        "id": 13,
        "designation": "Município do Belas",
        "provinceId": 3,
     
    },
    {
        "id": 14,
        "designation": "Município de Luanda",
        "provinceId": 3,
       
    }
]

document.getElementById("municipalitiesInProvince").textContent = JSON.stringify(municipalitiesInProvince, null, 2);


var retrieveMunicipality = {
    "id": 3,
    "designation": "City of Ekurhuleni Metropolitan Municipality",
    "provinceId": 2,
    "points": null,
    "isUpdateCoordinates": false
};

document.getElementById("retrieveMunicipality").textContent = JSON.stringify(retrieveMunicipality, null, 2);


var municipalitiesInProvince = [
    {
        "id": 4,
        "designation": "Município do Cacuaco",
        "provinceId": 3,
        "points": null,
        "isUpdateCoordinates": false
    },
    {
        "id": 13,
        "designation": "Município do Belas",
        "provinceId": 3,
        "points": null,
        "isUpdateCoordinates": false
    },
    {
        "id": 14,
        "designation": "Município de Luanda",
        "provinceId": 3,
        "points": null,
        "isUpdateCoordinates": false
    }
];

document.getElementById("municipalitiesInProvince").textContent = JSON.stringify(municipalitiesInProvince, null, 2);

/**
 * Subcategories
 * 
 */

var subcategoriesCreate = {
    "designation": "Offices",
    "categoryId": "1",
    "lng": "en"
};

document.getElementById("subcategoriesCreate").textContent = JSON.stringify(subcategoriesCreate, null, 2);

var allSubcategories = [
    {
        "id": 1,
        "designation": "Manufacturers",
        "categoryId": 2,
        "lng": "en"
    },
    {
        "id": 2,
        "designation": "Supermarket",
        "categoryId": 8,
        "lng": "en"
    },
    {
        "id": 4,
        "designation": "Offices",
        "categoryId": 1,
        "lng": "en"
    }
];

document.getElementById("allSubcategories").textContent = JSON.stringify(allSubcategories, null, 2);


var subcategoriesInCategory = [
    {
        "id": 1,
        "designation": "Manufacturers",
        "categoryId": 21,
        "lng": "en"
    },
    {
        "id": 1,
        "designation": "Industry",
        "categoryId": 34,
        "lng": "en"
    }
];


document.getElementById("subcategoriesInCategory").textContent = JSON.stringify(subcategoriesInCategory, null, 2);


var singleCategory = {
    "id": 3,
    "designation": "Assembleia",
    "categoryId": 9,
    "lng": "pt"
};

document.getElementById("singleCategory").textContent = JSON.stringify(singleCategory, null, 2);


var suburbBySector = [
    {
        "id": 2,
        "municipalityId": 1,
        "designation": "Carlswald"
    },
    {
        "id": 4,
        "municipalityId": 1,
        "designation": "Barbeque Downs"
    },
    {
        "id": 7,
        "municipalityId": 1,
        "designation": "Wilgeheuwel"
    },
    {
        "id": 13,
        "municipalityId": 1,
        "designation": "Benmore Gardens"
    },
    {
        "id": 14,
        "municipalityId": 1,
        "designation": "Wendywood"
    },
    {
        "id": 15,
        "municipalityId": 1,
        "designation": "Rivonia Gardens"
    },
    {
        "id": 16,
        "municipalityId": 1,
        "designation": "Johannesburg Ward 93"
    },
    {
        "id": 17,
        "municipalityId": 1,
        "designation": "Witkoppen Extensions"
    }
];

document.getElementById("suburbBySector").textContent = JSON.stringify(suburbBySector, null, 2);


var suburbById = {
    "id": 18,
    "municipalityId": 4,
    "designation": "Cacuaco"
};

document.getElementById("suburbById").textContent = JSON.stringify(suburbById, null, 2);




var suburbList = [
    {
        "id": 2,
        "designation": "Carlswald"
    },
    {
        "id": 3,
        "designation": "Hatfield"
    }
]

document.getElementById("suburbList").textContent = JSON.stringify(suburbList, null, 2);
document.getElementById("suburbListbyParam").textContent = JSON.stringify(suburbList, null, 2);