// miniprogram/pages/poi/poi.js
const F2 = require('@antv/wx-f2');
const db = wx.cloud.database();
const _ = db.command;
let chart = null;
let matrix = [];

function initChart(canvas, width, height, F2) { // 使用 F2 绘制图表
  // console.log(matrix);
  // const data = [
  //   { date: '4月1日', total: 20000 },
  //   { date: '4月2日', total: 25000 },
  //   { date: '4月3日', total: 30000 },
  //   { date: '4月4日', total: 28000 },
  //   { date: '4月5日', total: 10000 },
  //   { date: '4月6日', total: 10000 },
  //   { date: '4月7日', total: 10000 },
  // ];
  chart = new F2.Chart({
    el: canvas,
    width,
    height
  });

  chart.source(matrix, {
    sales: {
      tickCount: 5
    }
  });
  chart.tooltip({
    showItemMarker: false,
    onShow(ev) {
      const { items } = ev;
      items[0].name = null;
      items[0].name = items[0].title;
      items[0].value = items[0].value + '人';
    }
  });
  chart.interval().position('date*total');
  chart.render();
  return chart;
}

function convertMatrix(input) {
  matrix = [];
  var x;
  for (x of input) {
    var dateInt = x.date;
    var dateMonth = dateInt % 10000;
    var month = Math.floor(dateMonth / 100);
    var day = dateMonth % 100;
    var date = month + '月' + day + '日';
    console.log(date);
    var total = x.maxTraffic;
    console.log(total);
    matrix.push({ date: date, total: total });
  }
}

Page({

  /**
   * 页面的初始数据
   */
  data: {
    date: new Date().toJSON().slice(0, 10),
    traffic: '-',
    id: 0,
    poi: {},
    opts: {
      lazyLoad: true
    }
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {
    // console.log(options);
    this.loadData(options.id);
    this.data.id = options.id;
  },

  loadData: function(id) {
    var self = this;
    db.collection('Poi').doc(Number(id)).get()
      .then(res => {
        matrix = res.data.history;
        convertMatrix(res.data.history);
        self.chartComponent = self.selectComponent('#column-dom');
        self.chartComponent.init(initChart);

        self.setData({
          poi: res.data
        });
      });
  },

  bindDateChange: function (e) {
    // console.log('picker发送选择改变，携带值为', e.detail.value);
    this.setData({
      date: e.detail.value
    });
    // var dateString = this.data.date.replace(/-/g, '');
    // console.log(typeof this.data.date);
    // console.log(dateString);
  },

  forcastClick: function () {
    var dateString = this.data.date.replace(/-/g, '');
    var self = this;
    wx.request({
      url: 'https://happytripservice.azurewebsites.net/api/poi/' + self.data.id + '/forcast/' + dateString,
      header: {
        'content-type': 'application/json'
      },
      success(res) {
        console.log(res);
        self.setData({
          traffic: res.data.traffic
        });
      }
    })
  },
})