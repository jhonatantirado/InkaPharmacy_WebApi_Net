ALTER TABLE product 
ADD COLUMN image_url VARCHAR(300) NULL;

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accu-chek-instant-s-test-strips-50-s-835774208.jpg'
where product_id in (1,11,21,31,41,51,61,71);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accu-chek-active-test-strip-50-s-15580684.jpg'
where product_id in (81,91,101,111,121,131,141);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accusure-simple-gluco-test-strip-50s--857449581.jpg'
where product_id in (151,161,171,181,191,201,211);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accu-chek-instant-s-blood-glucose-monitor-1-s_812638_556.jpg'
where product_id in (221,231,241,251,261,271,281);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accusure-simple-gluco-meter-with-25-strips--857447929.jpg'
where product_id in (291,301,311,321,331,341,351);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/c/accu-chek-instant-s-test-strips-50-s-835774208.jpg'
where product_id in (291,301,311,321,331,341,351);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/e/n/ensure-diabetes-care-vanilla-powder-400gm-413072622.jpg'
where product_id in (361,371,381,391,401,411,421);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/v/avalife-diabetes-support-tablets-60-s-828634372.jpg'
where product_id in (431,441,451,461,471,481,491);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/p/apollo-sugar-glucome-glucometer-glucome-strips-25s-pack-diabetes-foot-wear-voucher-worth-rupees-500-1-s-846888757.jpg'
where product_id in (501,511,521,531,541,551,561);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/a/l/allen-a08-diabetes-drops-30-ml-830953142.jpg'
where product_id in (571,581,591,601,611,621,631);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/o/m/omeo-diabetes-drops-30ml-828541222.jpg'
where product_id in (641,651,661,671,681,691,701);

UPDATE product
set image_url='https://buybestmeds.com/content/160x120/viagra.jpg'
where product_id in (711,721,731,741,751,761,771);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/g/n/gnc-burn-caplet-60-s-813850403.jpg'
where product_id in (781,791,801,811,821,831,841);

UPDATE product
set image_url='https://ae01.alicdn.com/kf/H59d63c164b9c485abe078ef563fb23dde/2019-New-Mini-Hair-Straightener-Flat-Iron-Curling-Hair-Beard-Straightener-Comb-Hair-Crimper-Curling-Iron.jpg'
where product_id in (851,861,891,901,911,921,931);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/d/a/dabur-honey-50-gm-12729360.jpg'
where product_id in (941,951,961,971,981,991,1001);

UPDATE product
set image_url='https://www.netmeds.com/pub/media/catalog/product/cache/image/240x240/57453768b1c789093496adaa9e415a9d/s/r/srisri-tattva-amla-juice-1000ml-824343297.jpg'
where product_id in (871,881,1011,1021,1051,1071);

UPDATE customer
set document_number=telephone
where document_number is null;