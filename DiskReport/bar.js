var trace1 = {
  x: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'], 
  y: [20, 14, 25, 16, 18, 22, 19, 15, 12, 16, 14, 17],
  type: 'bar',
  name: 'Primary Product',
  marker: {
    color: 'rgb(49,130,189)',
    opacity: 0.7
  }
};

var data = [trace1];

var layout = {
  autosize: true,
  title: 'Error Report',
  height:400,
  width:400,
  xaxis: {
    tickangle: -45
  },
  barmode: 'group',
  paper_bgcolor: 'rgba(0,0,0,0)',
  plot_bgcolor: 'rgba(0,0,0,0)'
};

Plotly.newPlot('bar', data, layout);