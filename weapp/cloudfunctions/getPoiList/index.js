// 云函数入口文件
const cloud = require('wx-server-sdk')
var got = require('got');

cloud.init({
  env: 'yshlaptop'
});
let db = cloud.database();

// 云函数入口函数
exports.main = async (event, context) => {
  console.log('========== Update start ==========');

  const response = await got('https://happytripservice.azurewebsites.net/api/poi?includeHistory=true');
  // console.log(response.body.value);
  var result = JSON.parse(response.body);
  // console.log(result);
  result.value.forEach(function(poi) {
    var id = poi.id;
    db.collection('Poi').doc(id)
      .set({
          data: poi
        });
  });

  console.log('========== Update finish ==========');
}