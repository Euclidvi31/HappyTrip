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
    if (id == 48)
    {
      poi.background = 'https://7973-yshlaptop-1301185334.tcb.qcloud.la/backgrounds/disney.webp?sign=96fff8c05bc48fb37762216d493da0ab&t=1614698724';
    }
    else if (id == 95)
    {
      poi.background = 'https://7973-yshlaptop-1301185334.tcb.qcloud.la/backgrounds/huanlegu.webp?sign=00fc70f52811e515b73d85fb75d4f484&t=1614864067';
    }
    db.collection('Poi').doc(id)
      .set({
          data: poi
        });
  });

  console.log('========== Update finish ==========');
}