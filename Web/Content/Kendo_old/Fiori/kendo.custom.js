// Kendo UI theme for data visualization widgets
// Use as theme: 'newTheme' in configuration options (or change the name)
kendo.dataviz.ui.registerTheme('newTheme', {
    "chart": {
        "title": {
            "color": "#333333"
        },
        "legend": {
            "labels": {
                "color": "#333333"
            }
        },
        "chartArea": {},
        "seriesDefaults": {
            "labels": {
                "color": "#333333"
            }
        },
        "axisDefaults": {
            "line": {
                "color": "#c8c8c8"
            },
            "labels": {
                "color": "#333333"
            },
            "minorGridLines": {
                "color": "#dddddd"
            },
            "majorGridLines": {
                "color": "#c8c8c8"
            },
            "title": {
                "color": "#333333"
            }
        },
        "seriesColors": [
            "#008fd3",
            "#99d101",
            "#f39b02",
            "#f05662",
            "#c03c53",
            "#acacac"
        ],
        "tooltip": {}
    },
    "gauge": {
        "pointer": {
            "color": "#008fd3"
        },
        "scale": {
            "rangePlaceholderColor": "#c8c8c8",
            "labels": {
                "color": "#333333"
            },
            "minorTicks": {
                "color": "#333333"
            },
            "majorTicks": {
                "color": "#333333"
            },
            "line": {
                "color": "#333333"
            }
        }
    }
});