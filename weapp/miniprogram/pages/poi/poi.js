// miniprogram/pages/poi/poi.js
// import F2 from "@antv/wx-f2";

let chart = null;

function initChart(canvas, width, height, F2) { // 使用 F2 绘制图表
  const data = [
    { date: '4月1日', total: 20000 },
    { date: '4月2日', total: 25000 },
    { date: '4月3日', total: 30000 },
    { date: '4月4日', total: 28000 },
    { date: '4月5日', total: 10000 },
  ];
  chart = new F2.Chart({
    el: canvas,
    width,
    height
  });

  chart.source(data, {
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

Page({

  /**
   * 页面的初始数据
   */
  data: {
    opts: {
      onInit: initChart
    },
    date: new Date().toJSON().slice(0, 10)
  },

  /**
   * 生命周期函数--监听页面加载
   */
  onLoad: function (options) {

  },

  /**
   * 生命周期函数--监听页面初次渲染完成
   */
  onReady: function () {

  },

  /**
   * 生命周期函数--监听页面显示
   */
  onShow: function () {

  },

  /**
   * 生命周期函数--监听页面隐藏
   */
  onHide: function () {

  },

  /**
   * 生命周期函数--监听页面卸载
   */
  onUnload: function () {

  },

  /**
   * 页面相关事件处理函数--监听用户下拉动作
   */
  onPullDownRefresh: function () {

  },

  /**
   * 页面上拉触底事件的处理函数
   */
  onReachBottom: function () {

  },

  /**
   * 用户点击右上角分享
   */
  onShareAppMessage: function () {

  }
})