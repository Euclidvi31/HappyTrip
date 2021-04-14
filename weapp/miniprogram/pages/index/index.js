//index.js
const app = getApp();
const db = wx.cloud.database();
const _ = db.command;

Page({
  data: {
    inputShowed: false,
    inputVal: "",
    pois:[]
  },

  onLoad: function() {
    this.loadData();
  },

  showInput: function() {
  },

  openCard: function(e)
  {
    wx.navigateTo({
      url: '/pages/poi/poi?id=' + e.currentTarget.id,
    })
  },

  onPullDownRefresh: function () {
    wx.showLoading({title: '加载中', mask:true});
    this.loadData();
    //console.log("Refresh done.");
  },

  loadData: function() {
    var self = this;
    db.collection('Poi')
      .field(
        {
          _id: false,
          history: false
        })
      .orderBy('trafficNumber', 'desc')
      .get()
      .then(res =>{
        self.setData({
          'pois': res.data
        });
        wx.stopPullDownRefresh();
        wx.hideLoading({
          success: (res) => {},
        });
      })
  },

  onShareAppMessage: function () {
    return {
      // title: poi.name + ' 当前客流 ' + poi.traffic,
      path: '/pages/index/index'
    }
  },
})
