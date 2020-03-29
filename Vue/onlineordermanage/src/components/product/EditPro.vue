<template>
  <el-form ref="ruleForm" :model="form" :rules="rules" label-width="100px">
    <el-form-item label="产品名称" prop="proName">
      <el-input v-model="form.proName" placeholder="请输入产品名称" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品编码">{{this.form.proCode}}</el-form-item>
    <el-form-item label="产品分类" prop="categoryId">
      <el-cascader
        placeholder="请选择产品分类"
        v-model="form.categoryId"
        :options="procateoptions"
        :props="{ checkStrictly: true,emitPath:false }"
        clearable
        style="width:80%;"
      ></el-cascader>
    </el-form-item>
    <el-form-item label="产品基本单位">
      <el-input v-model="form.proBaseUnit" style="width:80%;"></el-input>
    </el-form-item>
    <el-form-item label="产品图片" prop="uploadId">
      <el-upload
        v-if="uploadVisible"
        :action="uploadAddr"
        :data="data"
        name="files"
        :file-list="fileList"
        :show-file-list="false"
        :auto-upload="true"
        :limit="1"
        :v-model="form.uploadId"
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
            <!-- 默认插槽 -->
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
    <el-form-item label="产品描述">
      <el-input type="textarea" v-model="form.proDesc"></el-input>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('ruleForm')">保存产品</el-button>
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
      uploadAddr: http.rootApi + "/upload/UploadImageAndGenerateThum",
      fit: "contain",
      url: "",
      disabled: false,
      uploadVisible: false,
      procateoptions: [],
      data: {
        limitSize: 80
      },
      fileList: [],
      form: {
        id: "",
        proName: "",
        proCode: "",
        proBaseUnit: "",
        categoryId: "",
        uploadId: "",
        proDesc: "",
        rowVersion: ""
      },
      rules: {
        proName: [
          { required: true, message: "请输入产品名称", trigger: "blur" }
        ],
        categoryId: [
          {
            required: true,
            message: "请选择产品分类",
            trigger: "change"
          }
        ],
        uploadId: [
          {
            required: true,
            message: "请上传图片",
            trigger: "change"
          }
        ]
      }
    };
  },
  components: {
    VueHoverMask
  },
  mounted() {
    //加载分类信息
    this.getCategoryList();
    this.form.id = this.$parent.$parent.currentId;
    //加载详情
    this.getProInfo();
  },
  methods: {
    uploadSuccess(data) {
      if (data.data && data.data.uploadId) {
        this.form.uploadId = data.data.uploadId;
        this.fileList = data.data.fileInfosResult;
        //判断是否获取到了上传文件
        if (this.fileList && this.fileList.length > 0) {
          for (var key in this.fileList) {
            this.fileList[key].url =
              "http://localhost:5002" + this.fileList[key].url;
          }
        } else {
          this.uploadVisible = true;
        }
        this.$refs["ruleForm"].validate();
      }
    },
    uploadExceed() {
      this.$message("只能上传一张图片");
    },
    getProInfo() {
      //获取分类详情信息
      var getApi = "/ProInfo/GetProInfo";
      var data = {
        id: this.form.id
      };
      http.get(getApi, JSON.stringify(data), response => {
        if (response.success) {
          //赋值
          this.form.proName = response.data.proName;
          this.form.proCode = response.data.proCode;
          this.form.proBaseUnit = response.data.proBaseUnit;
          this.form.categoryId = response.data.categoryId;
          this.form.proDesc = response.data.proDesc;
          this.form.rowVersion = response.data.rowVersion;
          //判断是否获取到了上传文件
          if (
            response.data.fileInfos &&
            response.data.fileInfos.length > 0
          ) {
            this.fileList = response.data.fileInfos;
            for (var key in this.fileList) {
              this.fileList[key].url =
                "http://localhost:5002" + this.fileList[key].url;
            }
          } else {
            this.uploadVisible = true;
          }
        } else {
          //弹出错误信息提示
          this.$message.error(response.errMsg);
        }
      });
    },
    getCategoryList() {
      var api = "/ProCategory/GetDropDownListAsync";
      http.get(api, null, response => {
        if (response && response.success && response.data) {
          this.procateoptions = response.data;
        } else {
          this.procateoptions.clear();
        }
      });
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
    handlePicturePreview(e) {
      if (e.target.dataset && e.target.dataset.pid) {
        for (var i in this.fileList) {
          //判断是否存在
          if (this.fileList[i].id == e.target.dataset.pid) {
            this.$parent.$parent.imageUrl =
              "http://localhost:5002" + this.fileList[i].sourceUrl;
            this.$parent.$parent.imgdialogVisible = true;
            break;
          }
        }
      }
    },
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        //验证通过
        if (valid) {
          var api = "/ProInfo/Edit";
          http.put(api, this.form, response => {
            if (response.success) {
              this.$message({
                message: "产品保存成功",
                type: "success"
              });
              //获取父级窗口是否正确
              if (this.$parent.$parent) {
                //关闭窗口
                this.$parent.$parent.dialogEditProduct = false;
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
        this.$parent.$parent.dialogEditProduct = false;
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