   var data = [{
        values: [20, 80],
        labels: ['Free', 'Used',],
        type: 'pie',
        hoverinfo: 'label+percent'
      }];

    var layout = {
      autosize: true,
      height: 300,
      width: 300,
      margin: {
        l: 0,
        r: 0,
        b: 0,
        t: 0
      },
      paper_bgcolor: 'rgba(0,0,0,0)',
      plot_bgcolor: 'rgba(0,0,0,0)'
    };
  Plotly.newPlot('pie', data, layout);