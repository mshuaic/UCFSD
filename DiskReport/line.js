var trace1 = {
  x: ['1', '4', '7', '10', '13', '16', '19', '22', '25', '28', '31', '34'], 
  y: [67, 71, 75, 80, 86, 90, 92, 95, 97, 100, 100, 100],
  mode: 'lines+markers',
};

var data = [trace1];

var layout = {
  autosize: true,
  title: 'Disk Usage',
  height:400,
  width:400,
  xaxis:{
	  title: 'Days from now',
  },
  yaxis:{
	  title: 'Percent Disk Space Utilized'
  }
};

Plotly.newPlot('line', data, layout);