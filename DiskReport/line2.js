var trace1 = {
  x: ['1', '4', '7', '10', '13', '16', '19', '22', '25', '28', '31', '34'], 
  y: [45,45.4,45.7,46.1,46.5,46.9,47.4,47.7,48.2,48.7,49.1,49.5],
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

Plotly.newPlot('line2', data, layout);