<template>
  <el-form ref="ruleForm" :model="form" :rules="rules" label-width="100px">
    <el-form-item label="供应商名称" prop="supplierName">
      <el-input v-model="form.supplierName" placeholder="请输入供应商名称" style="width:80%;"></el-input>
    </el-form-item>
    <!-- <el-form-item label="产品编码">
      <el-input v-model="form.proCode" placeholder="请输入产品编码" style="width:80%;"></el-input>
    </el-form-item>-->
    <el-form-item label="负责人" prop="contactMan">
      <el-input v-model="form.contactMan" placeholder="请输入负责人" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="联系电话" prop="contactPhone">
      <el-input v-model="form.contactPhone" placeholder="请输入联系电话" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="省市区" prop="districtValue">
      <el-cascader
        ref="districtSelect"
        placeholder="请选择省市区"
        v-model="form.districtValue"
        :options="districts"
        :props="{ emitPath:true, expandTrigger:'hover'}"
        clearable
        style="width:80%;"
        @change="districtChange"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="详情地址" prop="address">
      <el-input v-model="form.address" placeholder="请输入详情地址（不包括省市区）" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="营业执照" prop="businessLicense">
      <el-upload
        v-if="uploadVisible"
        :action="uploadAddr"
        :data="data"
        name="files"
        :auto-upload="true"
        :show-file-list="false"
        :limit="1"
        :v-model="form.businessLicense"
        :on-success="uploadSuccess"
        :on-exceed="uploadExceed"
      >
        <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
      </el-upload>
      <ul class="el-upload-list el-upload-list--picture">
        <li
          :tabindex="i"
          v-for="(item,i) in fileList"
          :key="i"
          class="el-upload-list__item is-success"
          style
        >
          <VueHoverMask>
            <img :src="item.url" alt class="el-upload-list__item-thumbnail" />
            <template v-slot:action>
              <i class="el-icon-zoom-in" @click="handlePicturePreview" :data-pid="item.id"></i>
            </template>
          </VueHoverMask>
          <a class="el-upload-list__item-name">
            <i class="el-icon-document"></i>
            {{item.name}}
          </a>
          <label class="el-upload-list__item-status-label">
            <i class="el-icon-upload-success el-icon-check"></i>
          </label>
          <i class="el-icon-close" @click="handleRemove" :data-pid="item.id"></i>
          <i class="el-icon-close-tip">按 delete 键可删除</i>
        </li>
      </ul>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('ruleForm')">立即创建</el-button>
      <el-button @click="onCancel">取消</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import http from "../../common/http/vueresource.js";
import VueHoverMask from "vue-hover-mask";
export default {
  data() {
    return {
      uploadVisible: true,
      data: {
        limitSize: 80
      },
      fileList: [],
      districts: [],

      uploadAddr: http.rootApi + "/api/upload/UploadImageAndGenerateThum",
      form: {
        districtValue: [],
        supplierName: "",
        contactMan: "",
        contactPhone: "",
        address: "",
        businessLicense: "",
        province: "",
        provinceName: "",
        city: "",
        cityName: "",
        district: "",
        districtName: ""
      },
      rules: {
        contactMan: [
          { required: true, message: "请输入负责人", trigger: "blur" }
        ],
        contactPhone: [
          { required: true, message: "请输入联系电话", trigger: "blur" }
        ],
        districtValue: [
          { required: true, message: "请选择省市区", trigger: "change" }
        ],
        address:[
          { required: true, message: "请输入详细地址", trigger: "blur" }
        ]
      }
    };
  },
  components: {
    VueHoverMask
  },
  mounted() {
    this.getDistrict();
  },
  methods: {
    districtChange() {
      var nodes = this.$refs["districtSelect"].getCheckedNodes();
      var node = undefined;
      if (nodes) {
        node = nodes[0];
      }
      if (node && node.pathLabels) {
        //赋值省市区
        this.form.province = this.form.districtValue[0];
        //存在地级市单位
        if (this.form.districtValue.length == 2) {
          this.form.city = this.form.districtValue[1];
        }
        //存在县级单位
        else if (this.form.districtValue.length == 3) {
          this.form.city = this.form.districtValue[1];
          this.form.district = this.form.districtValue[2];
        }

        //赋值省市区的名称
        this.form.provinceName = node.pathLabels[0];
        if (node.pathLabels.length == 2) {
          this.form.cityName = node.pathLabels[1];
        } else if (node.pathLabels.length == 3) {
          this.form.cityName = node.pathLabels[1];
          this.form.districtName = node.pathLabels[2];
        }
      } else {
        this.form.province = "";
        this.form.city = "";
        this.form.district = "";
        this.form.provinceName = "";
        this.form.cityName = "";
        this.form.districtName = "";
      }
    },
    getDistrict() {
      var api = "/api/District/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.districts = response.data;
        } else {
          this.districts.clear();
        }
      });
    },
    uploadSuccess(data) {
      if (data.data && data.data.uploadId) {
        this.form.businessLicense = data.data.uploadId;
        this.fileList = data.data.fileInfosResult;
        //判断是否获取到了上传文件
        if (this.fileList && this.fileList.length > 0) {
          this.uploadVisible = false;
          for (var key in this.fileList) {
            this.fileList[key].url = http.rootApi + this.fileList[key].url;
          }
        } else {
          this.uploadVisible = true;
        }
        //this.$refs["ruleForm"].validate();
      }
    },
    uploadExceed() {
      this.$message("只能上传一张图片");
    },
    handlePicturePreview(e) {
      if (e.target.dataset && e.target.dataset.pid) {
        for (var i in this.fileList) {
          //判断是否存在
          if (this.fileList[i].id == e.target.dataset.pid) {
            this.$parent.$parent.imageUrl =
              http.rootApi + this.fileList[i].sourceUrl;
            this.$parent.$parent.imgdialogVisible = true;
            break;
          }
        }
      }
    },
    handleRemove(e) {
      if (e.target.dataset && e.target.dataset.pid) {
        var index = -1;
        for (var i in this.fileList) {
          //判断是否存在
          if (this.fileList[i].id == e.target.dataset.pid) {
            index = i;
            break;
          }
        }
        if (index > -1) {
          this.fileList.splice(index, 1);
        }
        this.uploadVisible = true;
        this.form.uploadId = "";
        this.$refs["ruleForm"].validate();
      }
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        //验证通过
        if (valid) {
          var api = "/api/Supplier/Add";
          http.post(api, this.form, response => {
            if (response.success) {
              this.$message({
                message: "供应商添加成功",
                type: "success"
              });
              //获取父级窗口是否正确
              if (this.$parent.$parent) {
                //关闭窗口
                this.$parent.$parent.dialogAddSupplier = false;
                this.$parent.$parent.getData();
              }
            } else {
              this.$message.error(response.errMsg);
            }
          });
        } else {
          return false;
        }
      });
    },
    onCancel() {
      if (this.$parent.$parent) {
        this.$parent.$parent.dialogAddSupplier = false;
      }
    }
  }
};
</script>
<style scoped>
.el-upload-list .el-upload-list__item .vue-hover-mask {
  float: left;
  margin-left: -80px;
  height: 80px;
  width: 80px;
  margin-top: -5px;
}

.el-upload-list
  .el-upload-list__item
  .vue-hover-mask
  img.el-upload-list__item-thumbnail {
  position: absolute;
  left: 0;
  right: 0;
  top: 0;
  bottom: 0;
  margin: auto;
  height: auto;
  width: auto;
  float: none;
}
</style>