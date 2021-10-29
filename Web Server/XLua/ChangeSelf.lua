xlua.hotfix(CS.ChangeSelf, 'ChangeSelf', function (self, scale))
    -- body
    self.m_LaterScale = scale * 0.5
end)


local Quternion = CS.UnityEngine.Quternion
local rot = CS.UnityEngine.Quternion()  --新建一个对象
xlua.hotfix(CS.ChangeSelf, 'ChangeRotation', function(self, rotation)
    -- body
    self.m_LaterRotation = Quaternion.Euler(rotation.eulerAngles.x + 60, rotation.eulerAngles.y + 60, rotation.eulerAngles.z + 60)
end)