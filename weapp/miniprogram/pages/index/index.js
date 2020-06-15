//index.js
const app = getApp()

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
    wx.request({
      url: 'https://happytripservice.azurewebsites.net/api/poi',
      header: {
        'content-type': 'application/json' 
      },
      success(res) {
        //console.log(res.data.value);
        self.setData({
          'pois': res.data.value
        });
        wx.stopPullDownRefresh();
      }
    })
  },
})
