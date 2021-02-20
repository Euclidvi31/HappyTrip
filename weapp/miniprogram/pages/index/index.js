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
      })
  },
})
