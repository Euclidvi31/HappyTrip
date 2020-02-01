//index.js
const app = getApp()

Page({
  data: {
    inputShowed: false,
    inputVal: ""
  },

  onLoad: function() {
  },

  showInput: function() {
    console.info("showInput");
  },

  openCard: function(e)
  {
    wx.navigateTo({
      url: '/pages/poi/poi?id=' + e.currentTarget.id,
    })
  },
})
