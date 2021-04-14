// 云函数入口文件
const cloud = require('wx-server-sdk');
var got = require('got');

cloud.init({
  env: 'yshlaptop'
});
let db = cloud.database();

cloud.init()

// 云函数入口函数
exports.main = async (event, context) => {
  const response = await got('https://happytripservice.azurewebsites.net/api/poi/' + event.id + '/forcast/' + event.dateString);
  // console.log(response.body);
  var result = JSON.parse(response.body);
  return result;
}