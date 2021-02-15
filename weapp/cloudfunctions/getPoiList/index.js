// 云函数入口文件
const cloud = require('wx-server-sdk')
var request = require('request');

cloud.init()

// 云函数入口函数
exports.main = async (event, context) => {
  const wxContext = cloud.getWXContext()

  request('https://happytripservice.azurewebsites.net/api/poi', function (error, response, body) {
    if (!error && response.statusCode == 200) {
        console.log(body); // 输出请求到的body
        event.body = body;
    }
  });

  return {
    event,
    openid: wxContext.OPENID,
    appid: wxContext.APPID,
    unionid: wxContext.UNIONID,
  }
}